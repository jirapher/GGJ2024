using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AOEInputManager : MonoBehaviour
{
    private Camera cam;
    public PlayerController playerController;
    public LayerMask interactionLayer;
    public Vector3 mousePos;
    private Vector3 mouseRayReach = new Vector3(0, 0, 10);

    [Header("FollowerManagement")]
    private List<FollowerBehavior> selectedFollowers = new List<FollowerBehavior>();
    public FollowerManager followMan;

    [Header("Inputs")]
    public MainControls inputControls;
    private InputAction lmb, rmb;
    private Vector2 moveInput;

    [Header("Circles!")]
    public SelectionCircle circleSelect;
    public GameObject followCircle;

    private void Awake()
    {
        cam = Camera.main;
        circleSelect.gameObject.SetActive(false);
        inputControls = new MainControls();
    }

    private void OnEnable()
    {
        inputControls.PlayerControls.Enable();
        rmb = inputControls.PlayerControls.rmb;
        rmb.Enable();
        rmb.performed += MouseRight;

        lmb = inputControls.PlayerControls.lmb;
        lmb.Enable();
        lmb.performed += MouseLeftDown;
        lmb.canceled += MouseLeftUp;
    }

    private void OnDisable()
    {
        inputControls.PlayerControls.Disable();
    }

    private void Update()
    {
        moveInput = inputControls.PlayerControls.move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        playerController.MovePlayer(moveInput);
    }

    public void MouseLeftDown(InputAction.CallbackContext ctx)
    {
        CircleOn();
    }

    public void MouseLeftUp(InputAction.CallbackContext ctx)
    {
        CircleOff();
    }

    private void CircleOn()
    {
        circleSelect.gameObject.SetActive(true);
    }

    private void CircleOff()
    {
        circleSelect.gameObject.SetActive(false);
    }

    public void MouseRight(InputAction.CallbackContext ctx)
    {
        //raycast
        Vector3 clickLoc = cam.ScreenToWorldPoint(Input.mousePosition + mouseRayReach);
        clickLoc = new Vector2(clickLoc.x, clickLoc.y);
        Collider2D col = Physics2D.OverlapCircle(clickLoc, .15f, interactionLayer);

        if(col.TryGetComponent<EnemyBehavior>(out EnemyBehavior enemy))
        {
            print("setting attack enemy");
            //sticky but have to establish it's an enemy before we can pull unit health :/
            SetAttackTarget(enemy.GetComponent<UnitHealth>());
            return;
        }

        SetDestination(clickLoc);
    }

    public void AddUnit(FollowerBehavior unit)
    {
        unit.SetFollow(followCircle);

        if (selectedFollowers.Contains(unit)) { return; }

        selectedFollowers.Add(unit);
        unit.SetSelected(true);
    }

    public void RemoveUnit(FollowerBehavior unit)
    {
        if (selectedFollowers.Count < 1) { return; }

        if (selectedFollowers.Contains(unit))
        {
            selectedFollowers.Remove(unit);
            unit.SetSelected(false);
        }
    }

    public void ClearFollowers()
    {
        if (selectedFollowers.Count < 1) { selectedFollowers = new List<FollowerBehavior>(0); return; }

        foreach (FollowerBehavior unit in selectedFollowers)
        {
            unit.SetSelected(false);
        }
        selectedFollowers.Clear();
        selectedFollowers = new List<FollowerBehavior>(0);
    }

    public void SetFollow()
    {
        foreach(FollowerBehavior follower in selectedFollowers)
        {
            follower.SetFollow(followCircle);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        foreach (FollowerBehavior follower in selectedFollowers)
        {
            follower.SetDestination(destination);
        }
    }
    public void SetAttackTarget(UnitHealth enemy)
    {
        foreach (FollowerBehavior follower in selectedFollowers)
        {
            follower.SetAttack(enemy);
        }
    }

    public void SetAttackButton()
    {
        foreach (FollowerBehavior follower in selectedFollowers)
        {
            follower.SetAttack(null);
        }
    }

    public void SetIdle()
    {
        foreach (FollowerBehavior follower in selectedFollowers)
        {
            follower.SetIdle();
        }
    }


}
