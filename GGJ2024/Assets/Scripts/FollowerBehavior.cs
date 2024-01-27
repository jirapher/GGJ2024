using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowerBehavior : MonoBehaviour
{
    public enum Behavior { attack, follow, idle, move}
    public Behavior curBehavior;
    public bool isSelected = false;
    private Rigidbody2D rb;
    public GameObject highlight;
    private Camera cam;

    [Header("Attack")]
    public float timeBetweenAttacks = 1;
    private float attackTimeHold;
    public float attackRange = 1f;
    public float attackDamage = 1;
    private UnitHealth enemyTarget;
    private bool attacking = false;

    [Header("Move")]
    public float stoppingDistance = 1;
    public float moveSpeed = 1;
    private GameObject followTarget;
    private Vector3 destination = Vector3.zero;

    [Header("Detect")]
    public LayerMask interactableLayer;
    public float detectRadius;
    public float detectDistance;
    public float detectIntervalTime = 3;
    private float detectTimeHold;
    private void Start()
    {
        attackTimeHold = timeBetweenAttacks;
        detectTimeHold = detectIntervalTime;
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        FollowerManager.instance.AddFollower(this);
    }

    public void SetDestination(Vector3 d)
    {
        d.z = 0;
        destination = d;
        curBehavior = Behavior.move;
    }

    public void SetSelected(bool selected)
    {
        highlight.SetActive(selected);
        isSelected = selected;
    }

    private void Update()
    {

        if (attacking)
        {
            if(enemyTarget == null)
            {
                attacking = false;
                SetFollow(followTarget);
            }
            timeBetweenAttacks -= Time.deltaTime;
            if (timeBetweenAttacks <= 0)
            {
                //attack
                Attack();
                timeBetweenAttacks = attackTimeHold;
            }
            GetInAttackRange();
            return;
        }

        switch (curBehavior)
        {
            case Behavior.attack:
                if (enemyTarget == null)
                {
                    FollowMouse();
                    DetectTimer();
                    return;
                }
                GetInAttackRange();
                break;

            case Behavior.follow:
                if(followTarget == null) { return; }
                Follow();
                break;

            case Behavior.idle:
                rb.velocity = Vector2.zero;
                break;

            case Behavior.move:
                MoveToDestination();
                break;

        }
    }

    private void DetectTimer()
    {
        detectIntervalTime -= Time.deltaTime;
        if(detectIntervalTime < 0)
        {
            detectIntervalTime = detectTimeHold;
            TargetDetection();
        }
    }
    public void SetIdle()
    {
        curBehavior = Behavior.idle;
        highlight.GetComponent<SpriteRenderer>().color = Color.clear;
    }

    public void SetFollow(GameObject target)
    {
        followTarget = target;
        curBehavior = Behavior.follow;
        highlight.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void SetAttack(UnitHealth target)
    {
        enemyTarget = target;
        highlight.GetComponent<SpriteRenderer>().color = Color.red;
        curBehavior = Behavior.attack;
    }

    private void Attack()
    {
        enemyTarget.TakeDamage(attackDamage);
    }

    private void GetInAttackRange()
    {
        if ((enemyTarget.gameObject.transform.position - transform.position).sqrMagnitude > attackRange)
        {
            Vector2 dir = (enemyTarget.gameObject.transform.position - transform.position).normalized * moveSpeed;
            rb.velocity = dir;
            attacking = false;
            return;
        }

        attacking = true;
    }

    private void Follow()
    {
        if((followTarget.transform.position - transform.position).sqrMagnitude <= stoppingDistance)
        {
            return;
        }

        Vector2 dir = (followTarget.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = dir;
    }
    private void FollowMouse()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos = new Vector2(mousePos.x, mousePos.y);
        if (Vector2.Distance(mousePos, transform.position) <= stoppingDistance)
        {
            return;
        }

        Vector2 dir = (mousePos - transform.position).normalized * moveSpeed;
        rb.velocity = dir;
    }
    private void MoveToDestination()
    {
        if((destination - transform.position).sqrMagnitude <= stoppingDistance)
        {
            rb.velocity = Vector2.zero;
            SetIdle();
            return;
        }

        //else move
        Vector2 dir = (destination - transform.position).normalized * moveSpeed;
        rb.velocity = dir;
    }

    private void TargetDetection()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, detectRadius, Vector2.zero, detectDistance, interactableLayer);
        if(hit.Length > 0)
        {
            for(int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.TryGetComponent<EnemyBehavior>(out EnemyBehavior enemy))
                {
                    enemyTarget = enemy.GetComponent<UnitHealth>();
                    return;
                }
            }
        }
    }
}
