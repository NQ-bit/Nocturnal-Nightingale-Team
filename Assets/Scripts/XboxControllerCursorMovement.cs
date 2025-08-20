using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class XboxControllerCursorMovement : MonoBehaviour
{
public static XboxControllerCursorMovement Instance { get; private set; }

    [Header("Input System")]
    [SerializeField] InputActionAsset playerInput;
    [SerializeField] float clickCooldown = 0.5f;

    private InputAction moveAction;
    private float lastClickTime = 0f;

    void Awake()
    {
       /* if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }*/
            
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

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 joystick = context.ReadValue<Vector2>();

        // Convert joystick input to screen position
        Vector2 screenPos = new Vector2(
            (joystick.x + 1f) / 2f * Screen.currentResolution.width,
            (1f - (joystick.y + 1f) / 2f) * Screen.currentResolution.height
        );

        Mouse.current.WarpCursorPosition(screenPos);
        Debug.Log($"Using Xbox Controller cursor at {screenPos}");

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log($"Pressing Jump button");
        }

        // Raycast to detect Yarn Spinner buttons
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
    }


}
