using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    public float moveSpeed = 1;
    private Rigidbody2D rb;

    public Animator anim;
    private Vector2 lastRBVel = Vector2.zero;
    private bool isIdle = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer(Vector2 dir)
    {
        rb.velocity = dir * moveSpeed;
        isIdle = rb.velocity == Vector2.zero;
        anim.SetBool("isIdle", isIdle);
        if (isIdle)
        {
            IdleAnim();
            return;
        }

        WalkAnim();
        lastRBVel = rb.velocity;
    }
    private void IdleAnim()
    {
        anim.SetFloat("lastVX", lastRBVel.x);
        anim.SetFloat("lastVY", lastRBVel.y);
    }
    private void WalkAnim()
    {
        anim.SetFloat("vX", rb.velocity.x);
        anim.SetFloat("vY", rb.velocity.y);
    }
}
