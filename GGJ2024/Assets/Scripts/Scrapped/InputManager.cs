using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public MainControls inputControls;
    private InputAction lmb, rmb;
    private Vector2 moveInput;
    public PlayerController playerController;
    public LayerMask interactionLayer;
    private Camera cam;
    private Vector3 mouseRayReach = new Vector3(0, 0, 10);
    [SerializeField] private List<FollowerBehavior> selectedFollowers = new List<FollowerBehavior>();
    public FollowerManager followMan;

    [Header("Drag RTS-Like Selection")]
    public bool enableDragSelection = false;
    public DragSelection dragSelection;

    [Header("AOE Selection")]
    public CircleCollider2D aoeCol;


    private void Awake()
    {
        inputControls = new MainControls();
        cam = Camera.main;
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

        if (enableDragSelection) { lmb.canceled += DragCancel; }
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

    public void StartDragSelection()
    {
        if (dragSelection.isDragging) { return; }
        dragSelection.mousePos = Input.mousePosition;
        dragSelection.isDragging = true;
    }

    public void DragCancel(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled) { return; }
        if (dragSelection.isDragging)
        {
            ClearSelection();
            foreach(FollowerBehavior unit in followMan.GetAllFollowers())
            {
                if (!unit.isSelected)
                {
                    if (dragSelection.IsWithinSelectionBounds(unit.transform))
                    {
                        SelectFollower(unit);
                    }
                }
            }
        }

        dragSelection.isDragging = false;
    }

    private void CTXDragCancel()
    {
        if (dragSelection.isDragging)
        {
            ClearSelection();
            foreach (FollowerBehavior unit in followMan.GetAllFollowers())
            {
                if (!unit.isSelected)
                {
                    if (dragSelection.IsWithinSelectionBounds(unit.transform))
                    {
                        SelectFollower(unit);
                    }
                }
            }
            
        }

        dragSelection.isDragging = false;
    }

    public void MouseLeftDown(InputAction.CallbackContext ctx)
    {
        Vector3 clickLoc = cam.ScreenToWorldPoint(Input.mousePosition + mouseRayReach);
        clickLoc = new Vector2(clickLoc.x, clickLoc.y);
        Collider2D col = Physics2D.OverlapCircle(clickLoc, .15f, interactionLayer);

        if(col == null)
        {
            if (enableDragSelection)
            {
                CTXDragCancel();
                StartDragSelection();
                //return;
            }

            ClearSelection();
            return;
        }
        //check if it hit a follower
        if(col.TryGetComponent<FollowerBehavior>(out FollowerBehavior follower))
        {
            print(col.name + " is selected");
            SelectFollower(follower);
            return;
        }
    }

    public void MouseRight(InputAction.CallbackContext ctx)
    {
        if(selectedFollowers.Count > 0)
        {
            //raycast check for enemy first

            foreach(FollowerBehavior follower in selectedFollowers)
            {
                follower.SetDestination(cam.ScreenToWorldPoint(Input.mousePosition + mouseRayReach));
            }
        }
    }

    public void SelectFollower(FollowerBehavior unit)
    {
        if (selectedFollowers.Contains(unit))
        {
            DeselectFollower(unit);
            return;
        }

        selectedFollowers.Add(unit);
        unit.SetSelected(true);
    }

    public void DeselectFollower(FollowerBehavior unit)
    {
        if (selectedFollowers.Count < 1) { return; }

        if (selectedFollowers.Contains(unit))
        {
            selectedFollowers.Remove(unit);
            unit.SetSelected(false);
        }
    }

    public void ClearSelection()
    {
        print("clearselect");
        if(selectedFollowers.Count < 1) { selectedFollowers = new List<FollowerBehavior>(0); return; }

        foreach(FollowerBehavior unit in selectedFollowers)
        {
            unit.SetSelected(false);
            //selectedFollowers.Remove(unit);
        }
        selectedFollowers.Clear();
        selectedFollowers = new List<FollowerBehavior>(0);
    }

}
