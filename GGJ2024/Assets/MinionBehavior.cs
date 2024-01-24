using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehavior : MonoBehaviour
{
    public Transform GatherLocation;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var direction = GatherLocation.position - transform.position;
        rb.AddForce(new Vector2(direction.x, direction.y));
    }
}
