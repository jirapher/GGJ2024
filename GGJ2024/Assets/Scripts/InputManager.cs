using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputControls inputControls;
    private InputAction lmb, rmb;
    private Vector2 moveInput;
    public PlayerController playerController;
    public LayerMask interactionLayer;
    private Camera cam;
    private Vector3 mouseRayReach = new Vector3(0, 0, 10);

    public List<FollowerBehavior> selectedFollowers = new List<FollowerBehavior>();

    private void Awake()
    {
        inputControls = new InputControls();
        cam = Camera.main;
    }

    private void OnEnable()
    {
        inputControls.MainControls.Enable();
        
        rmb = inputControls.MainControls.RMB;
        rmb.Enable();
        rmb.performed += MouseRight;

        lmb = inputControls.MainControls.LMB;
        lmb.Enable();
        lmb.performed += MouseLeft;
    }

    private void OnDisable()
    {
        inputControls.MainControls.Disable();
    }

    private void FixedUpdate()
    {
        moveInput = inputControls.MainControls.Movement.ReadValue<Vector2>();
        playerController.MovePlayer(moveInput);
    }

    public void MouseLeft(InputAction.CallbackContext ctx)
    {
        Vector3 clickLoc = cam.ScreenToWorldPoint(Input.mousePosition + mouseRayReach);
        print(clickLoc);
        clickLoc = new Vector2(clickLoc.x, clickLoc.y);
        Collider2D col = Physics2D.OverlapCircle(clickLoc, .2f, interactionLayer);

        if (col == null) { return; }

        //check if it hit a follower
        if(col.TryGetComponent<FollowerBehavior>(out FollowerBehavior follower))
        {
            print(col.name + " is selected");
            SetSelected(follower);
        }


    }

    public void MouseRight(InputAction.CallbackContext ctx)
    {
        if(selectedFollowers.Count > 0)
        {
            foreach(FollowerBehavior follower in selectedFollowers)
            {
                follower.SetDestination(cam.ScreenToWorldPoint(Input.mousePosition + mouseRayReach));
            }
        }
    }

    private void SetSelected(FollowerBehavior unit)
    {
        if (selectedFollowers.Contains(unit)) { return; }

        selectedFollowers.Add(unit);
    }

    public void ClearSelection()
    {
        selectedFollowers.Clear();
    }

}
