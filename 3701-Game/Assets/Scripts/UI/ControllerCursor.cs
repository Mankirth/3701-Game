using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerCursor : MonoBehaviour
{
    [SerializeField]
    private float cursorSpeed = 10.0f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        Vector2 stickValue = Gamepad.current.leftStick.ReadValue();
        Debug.Log("STIOCK:" + stickValue);
        Vector2 currentPos = Mouse.current.position.ReadValue();
        Vector2 newPos = currentPos + stickValue * cursorSpeed;

        // Moves the actual OS cursor
        Mouse.current.WarpCursorPosition(newPos);
    }

}
