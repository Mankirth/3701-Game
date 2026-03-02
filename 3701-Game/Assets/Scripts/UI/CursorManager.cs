using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D clickedCursor;
    public Vector2 cursorHotspot = Vector2.zero;


    public InputAction mouseClick;

    private void Start()
    {
        mouseClick = new InputAction("Click", type: InputActionType.Button);
        mouseClick.AddBinding("<Mouse>/leftButton");
        mouseClick.Enable();

        //connect signals (press and not press) to proper functions that change cursor
        mouseClick.canceled += ChangeCursorDefault;
        mouseClick.performed += ChangeCursorClicked;
 
    }

   
    private void ChangeCursorClicked(InputAction.CallbackContext context)
    {
        // Change to the clicked cursor texture on press
        Cursor.SetCursor(clickedCursor, cursorHotspot, CursorMode.Auto);
    }

    private void ChangeCursorDefault(InputAction.CallbackContext context)
    {
        // Revert to the default cursor texture on release
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }


}
