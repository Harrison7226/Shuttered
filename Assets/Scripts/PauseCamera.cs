using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu; // Reference to the pause menu UI
    [SerializeField] private MonoBehaviour cameraControlScript; // Reference to the camera control script
    [SerializeField] private KeyCode pauseKey = KeyCode.P; // Key to pause/unpause

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game
        pauseMenu.SetActive(true); // Enable the pause menu
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
        if (cameraControlScript != null)
            cameraControlScript.enabled = false; // Disable camera control
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false); // Disable the pause menu
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back to the center
        Cursor.visible = false; // Hide the cursor
        if (cameraControlScript != null)
            cameraControlScript.enabled = true; // Re-enable camera control
    }
}
