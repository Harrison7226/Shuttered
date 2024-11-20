using UnityEngine;

public class CursorToggleController : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey = KeyCode.P; // Key to toggle cursor
    private bool isCursorVisible = false; // Initial cursor state

    void Update()
    {
        // Check for the key press
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleCursor();
        }
    }

    private void ToggleCursor()
    {
        // Toggle the cursor state
        isCursorVisible = !isCursorVisible;

        // Show or hide the cursor
        Cursor.visible = isCursorVisible;

        // Lock or unlock the cursor
        Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
