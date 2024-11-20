using UnityEngine;

public class UIToggleController : MonoBehaviour
{
    [SerializeField] private GameObject targetUI; // The UI element to enable/disable
    [SerializeField] private KeyCode toggleKey = KeyCode.P; // Key to toggle the UI

    void Update()
    {
        // Check for the key press
        if (Input.GetKeyDown(toggleKey))
        {
            if (targetUI != null)
            {
                // Toggle the UI element's active state
                targetUI.SetActive(!targetUI.activeSelf);
            }
        }
    }
}
