using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn;
using Yarn.Unity.Legacy;


public class XboxMovementScript : MonoBehaviour
{
    [Header("Cursor Setup")]
    public RectTransform cursor;
    public float moveSpeed = 1000f;
    public float deadzone = 0.2f;
    public float clickCooldown = 0.5f;

    [Header("Input System")]
    [SerializeField] InputActionAsset playerInput;

    private InputAction moveAction;
    private Vector2 cursorPosition;
    private float lastClickTime = 0f;

    private Vector2 currentStickValue; // stores the stick value

    void Awake()
    {
        // Find the Vector2 action bound to any stick
        foreach (var map in playerInput.actionMaps)
        {
            foreach (var action in map.actions)
            {
                if (action.expectedControlType == "Vector2" &&
                    action.bindings.Any(b => b.path.Contains("stick")))
                {
                    moveAction = action;
                    break;
                }
            }
            if (moveAction != null) break;
        }

        if (moveAction == null)
        {
            Debug.LogError("No joystick Vector2 action found in InputActionAsset.");
            return;
        }

        // Subscribe only to InputAction events
        moveAction.performed += OnMove;
        moveAction.canceled += OnMoveCanceled;
    }

    void OnEnable()
    {
        playerInput?.Enable();
    }

    void OnDisable()
    {
        playerInput?.Disable();
    }

    void Start()
    {
        cursorPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        cursor.position = cursorPosition;
    }

    // Called when stick is moved
    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();

        if (value.magnitude < deadzone)
            value = Vector2.zero;

        currentStickValue = new Vector2(value.x, -value.y);  // invert Y for UI
    }

    // Called when stick is released
    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        currentStickValue = Vector2.zero;
    }

    void Update()
    {
        MoveCursor();
        HandleClick();
    }

    private void MoveCursor()
    {
        if (currentStickValue == Vector2.zero)
            return;

        cursorPosition += currentStickValue * moveSpeed * Time.deltaTime;

        cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0, Screen.width);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0, Screen.height);

        cursor.position = cursorPosition;
    }

    private void HandleClick()
    {
        Gamepad pad = Gamepad.current;
        if (pad == null) return;

        if (pad.buttonSouth.wasPressedThisFrame &&
            Time.time - lastClickTime > clickCooldown)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = cursorPosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    ExecuteEvents.Execute(button.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
                    Debug.Log($"Clicked UI Button: {button.name}");

                    lastClickTime = Time.time;
                    break;
                }
            }
        }
    }
}
