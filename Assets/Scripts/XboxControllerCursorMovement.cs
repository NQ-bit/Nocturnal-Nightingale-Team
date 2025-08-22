using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class XboxControllerCursorMovement : MonoBehaviour
{
public static XboxControllerCursorMovement Instance { get; private set; }

    [Header("Input System")]
    [SerializeField] InputActionAsset playerInput;
    [SerializeField] float clickCooldown = 0.5f;
    [SerializeField] float cursorSpeed = 1000f;


    private InputAction moveAction;
    private float lastClickTime = 0f;
    private VirtualMouseInput virtualMouseInput;
    private Vector2 currentMousePos;



    void Awake()
    {

       if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        virtualMouseInput = GetComponent<VirtualMouseInput>();
        Debug.Log($"### mouse input: {virtualMouseInput}");

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

        if (moveAction != null)
        {
            moveAction.performed += OnLook;
        }
        else
        {
            Debug.LogError("No joystick Vector2 action found in InputActionAsset.");
        }
    }

    void OnEnable()
    {
        playerInput?.Enable();
    }

    void OnDisable()
    {
        playerInput?.Disable();
    }

    private void LateUpdate()
    {
        if (virtualMouseInput?.virtualMouse?.position == null)
        {
            Debug.LogError("Virtual mouse or its position is not initialized.");
            return;
        }

        Vector2 virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0f, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0f, Screen.height);
        InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePosition);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 joystick = context.ReadValue<Vector2>();

        Vector2 delta = joystick * cursorSpeed * Time.deltaTime;
        currentMousePos += delta;

        currentMousePos.x = Mathf.Clamp(currentMousePos.x, 0f, Screen.width);
        currentMousePos.y = Mathf.Clamp(currentMousePos.y, 0f, Screen.height);

        Debug.Log($"Xbox Controller moved cursor to {currentMousePos}");

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("Jump button pressed");

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = currentMousePos
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                GameObject target = results[0].gameObject;

                if (target.GetComponent<Button>() != null && Time.time - lastClickTime > clickCooldown)
                {
                    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);
                    Debug.Log($"Auto-clicked on {target.name} via joystick");
                    lastClickTime = Time.time;
                }
            }
        }




        /*if (Input.GetButtonDown("Jump"))
        {
            Debug.Log($"Pressing Jump button");

            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = screenPos
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                GameObject target = results[0].gameObject;

                if (target.GetComponent<Button>() != null && Time.time - lastClickTime > clickCooldown)
                {
                    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);
                    Debug.Log($"Auto-clicked on {target.name} via joystick");
                    lastClickTime = Time.time;
                }
            }

        } */

    }
}
