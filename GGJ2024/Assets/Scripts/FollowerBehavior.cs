using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBehavior : MonoBehaviour
{
    public enum Behavior { attack, follow, idle, move}

    public Behavior curBehavior;

    public float stoppingDistance = 1;
    public float moveSpeed = 1;

    private GameObject player;
    private GameObject enemyTarget;
    private Vector3 destination = Vector3.zero;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPlayer(GameObject p)
    {
        player = p;
    }

    public void SetTarget(GameObject t)
    {
        enemyTarget = t;
    }

    public void SetDestination(Vector3 d)
    {
        d.z = 0;
        destination = d;
        curBehavior = Behavior.move;
    }

    private void Update()
    {
        switch (curBehavior)
        {
            case Behavior.attack:
                break;

            case Behavior.follow:
                break;

            case Behavior.idle:
                break;

            case Behavior.move:
                MoveToDestination();
                break;

        }
    }

    private void GoIdle()
    {
        curBehavior = Behavior.idle;
    }

    private void MoveToDestination()
    {

        if((destination - transform.position).sqrMagnitude <= stoppingDistance)
        {
            rb.velocity = Vector2.zero;
            GoIdle();
            return;
        }

        //else move
        Vector2 dir = (destination - transform.position).normalized * moveSpeed;
        rb.velocity = dir;
    }
}
