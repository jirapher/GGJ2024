using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum Behavior { attack, follow, patrol, move }
    public Behavior curBehavior;
    public bool isSelected = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public GameObject highlight;
    private Camera cam;

    [Header("Patrol")]
    public float timeToMove = 1;
    public float timeBetweenMoves = 1;
    private float timeMoveHold, timeBetweenMovesHold;
    private bool patrolSet = false;

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
        timeMoveHold = timeToMove;
        timeBetweenMovesHold = timeBetweenMoves;
        attackTimeHold = timeBetweenAttacks;
        detectTimeHold = detectIntervalTime;
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
        highlight.GetComponent<SpriteRenderer>().color = Color.yellow;
        isSelected = selected;
    }

    private void Update()
    {

        if (attacking)
        {
            if (enemyTarget == null)
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
                    DetectTimer();
                    return;
                }
                GetInAttackRange();
                break;

            case Behavior.follow:
                if (followTarget == null) { return; }
                Follow();
                break;

            case Behavior.patrol:
                if (!patrolSet) { SetPatrol(); }
                MoveToDestination();
                DetectTimer();
                break;

            case Behavior.move:
                MoveToDestination();
                break;

        }
    }

    private void DetectTimer()
    {
        detectIntervalTime -= Time.deltaTime;
        if (detectIntervalTime < 0)
        {
            detectIntervalTime = detectTimeHold;
            TargetDetection();
        }
    }
    public void SetPatrol()
    {
        curBehavior = Behavior.patrol;
        highlight.GetComponent<SpriteRenderer>().color = Color.clear;
        float diceX = Random.Range(-3, 3);
        float diceY = Random.Range(-3, 3);
        destination = new Vector3(transform.position.x + diceX, transform.position.y + diceY, 0);
        patrolSet = true;
    }
    private void MoveToDestination()
    {
        if ((destination - transform.position).sqrMagnitude <= stoppingDistance)
        {
            //reach destination
            rb.velocity = Vector2.zero;
            if (patrolSet)
            {
                timeBetweenMoves -= Time.deltaTime;
                if(timeBetweenMoves < 0)
                {
                    timeBetweenMoves = timeBetweenMovesHold;
                    patrolSet = false;
                    SetPatrol();
                }
                return;
            }
            SetPatrol();
            return;
        }

        //else move
        if (patrolSet)
        {
            timeToMove -= Time.deltaTime;
            if(timeToMove < 0)
            {
                timeToMove = timeMoveHold;
                patrolSet = false;
                SetPatrol();
            }
        }
        Vector2 dir = (destination - transform.position).normalized * moveSpeed;
        rb.velocity = dir;
    }

    private void TargetDetection()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, detectRadius, Vector2.zero, detectDistance, interactableLayer);
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.TryGetComponent<FollowerBehavior>(out FollowerBehavior enemy))
                {
                    enemyTarget = enemy.GetComponent<UnitHealth>();
                    return;
                }
            }
        }
    }

    public void SetStats(FollowerStats stats)
    {
        //4dir?
        sr.sprite = stats.sprite;
        curBehavior = Behavior.patrol;
        timeBetweenAttacks = stats.timeBetweenAttacks;
        attackTimeHold = timeBetweenAttacks;
        attackRange = stats.attackRange;
        attackDamage = stats.attackDamage;
        stoppingDistance = stats.stoppingDistance;
        moveSpeed = stats.moveSpeed;
        interactableLayer = stats.interactableLayer;
        detectRadius = stats.detectRadius;
        detectDistance = stats.detectDistance;
        detectIntervalTime = stats.detectIntervalTime;
        detectTimeHold = detectIntervalTime;
        GetComponent<UnitHealth>().SetHealth(stats.maxHP);
    }

    #region Attack
    public void SetAttack(UnitHealth target)
    {
        enemyTarget = target;
        highlight.GetComponent<SpriteRenderer>().color = Color.red;
        curBehavior = Behavior.attack;
    }

    private void Attack()
    {
        enemyTarget.TakeDamage(attackDamage, GetComponent<UnitHealth>());
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
    #endregion

    #region Follow
    public void SetFollow(GameObject target)
    {
        followTarget = target;
        curBehavior = Behavior.follow;
        highlight.GetComponent<SpriteRenderer>().color = Color.green;
    }
    private void Follow()
    {
        if ((followTarget.transform.position - transform.position).sqrMagnitude <= stoppingDistance)
        {
            return;
        }

        Vector2 dir = (followTarget.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = dir;
    }
    #endregion
}
