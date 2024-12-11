using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoDetection : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    public static PhotoDetection Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Enforce single instance
        }
    }

    public void DetectPhotographedObject()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.GetComponent<PhotoTarget>())
            {
                hitObject.GetComponent<PhotoTarget>().OnPhotographed();
            }
        }
    }
}
