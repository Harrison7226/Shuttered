using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField] private GameObject objectivesUI;
    [SerializeField] private AudioSource toggleSFX;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggleSFX.Play();
            objectivesUI.SetActive(!objectivesUI.activeSelf);
        }
    }
}
