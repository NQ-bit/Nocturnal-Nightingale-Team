using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class XboxMovementScript : MonoBehaviour
{
    public static XboxControllerCursorMovement Instance { get; private set; }

    [Header("Input System")]
    [SerializeField] InputActionAsset playerInput;
    [SerializeField] float cursorSpeed = 1500f;

    private InputAction moveAction;

    void Awake()
    {
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

    #region Void Look Prev
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

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
    }
    #endregion
}
