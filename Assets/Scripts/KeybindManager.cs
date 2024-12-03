using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeybindManager : MonoBehaviour
{
    [System.Serializable]
    public class Keybind
    {
        public string ActionName; // Name of the action (e.g., "Jump")
        public KeyCode Key;       // Assigned key
    }

    public List<Keybind> keybinds = new List<Keybind>(); // List of keybinds
    public TMP_Text[] keybindTexts;                     // TextMeshPro UI displaying current keybinds

    private string selectedAction = null;   

    private KeyCode previousKey;

    public static KeybindManager Instance; // Singleton instance


    void Start()
    {
        // Initialize keybind texts on startup
        for (int i = 0; i < keybinds.Count; i++)
        {
            UpdateKeybindText(i);
        }
    }
        void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates if they exist
        }
    }

    void UpdateKeybindText(int index)
    {
        if (index >= 0 && index < keybindTexts.Length)
        {
            keybindTexts[index].text = keybinds[index].Key.ToString();
        }
    }

    public void StartRebinding(string action)
    {
        // Set the action to be rebound
        selectedAction = action;

        // Find the corresponding TextMeshPro text and show a prompt
        for (int i = 0; i < keybinds.Count; i++)
        {
            if (keybinds[i].ActionName == action)
            {
                keybindTexts[i].text = "-";
                previousKey = keybinds[i].Key;
                break;
            }
        }
    }

    void OnGUI()
    {
        // Check if we're waiting for a key press
        if (selectedAction != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                // Assign the pressed key to the selected action
                for (int i = 0; i < keybinds.Count; i++)
                {
                    if (keybinds[i].ActionName == selectedAction)
                    {
                        if (e.keyCode != KeyCode.Escape) {
                            keybinds[i].Key = e.keyCode; // Update keybind
                            UpdateKeybindText(i);
                        }
                        else {
                            keybinds[i].Key = previousKey;
                            UpdateKeybindText(i);
                        }
                        break;
                    }
                }

                selectedAction = null; // Clear the selection
            }
        }
    }
    public KeyCode GetKeyForAction(string actionName) {
    foreach (var keybind in keybinds)
    {
        if (keybind.ActionName == actionName)
        {
            return keybind.Key; // Return the KeyCode bound to the action
        }
    }
    return KeyCode.None; // Return None if the action is not found
    }
    
}
