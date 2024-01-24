using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehavior : MonoBehaviour
{
    public Transform GatherLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var direction = GatherLocation.position - transform.position;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x, direction.y));
    }
}
