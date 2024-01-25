using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    public float moveSpeed = 1;
    private Rigidbody2D rb;


    public List<FollowerBehavior> followers = new List<FollowerBehavior>();
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer(Vector2 dir)
    {
        rb.velocity = dir * moveSpeed;
    }
}
