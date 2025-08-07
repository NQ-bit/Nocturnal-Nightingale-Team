using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class XboxControllerCursorMovement : MonoBehaviour
{
    public static XboxControllerCursorMovement Instance { get; private set; }
    [SerializeField] InputActionAsset playerInput;
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
        }
        playerInput["Look"].performed += OnLook;
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 curr = context.ReadValue<Vector2>();
        Mouse.current.WarpCursorPosition(curr);
        Debug.Log("Using Xbox Controller cursor");
    }
}
