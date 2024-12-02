using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotoTarget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetName;

    public void OnPhotographed()
    {
        targetName.alpha = 0.1f;
        targetName.text = $"<s>{targetName.text}</s>";
    }
}
