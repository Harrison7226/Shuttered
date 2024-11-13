using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}