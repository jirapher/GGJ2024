using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCircle : MonoBehaviour
{
    public AOEInputManager input;

    public float radius = 4; //radius of bounds
    private Vector3 centerPosition;
    private float distance; //distance from ~green object~ to *black circle*
    public GameObject player;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        centerPosition = player.transform.position;
        Vector3 mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        distance = Vector3.Distance(mousePos, centerPosition);
        if (distance > radius) //If the distance is less than the radius, it is already within the circle.
        {
            Vector3 fromOriginToObject = mousePos - centerPosition;
            fromOriginToObject *= radius / distance;
            transform.position = centerPosition + fromOriginToObject;
        }
        else
        {
            transform.position = mousePos;
        }
    }
}
