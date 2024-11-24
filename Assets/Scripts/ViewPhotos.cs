using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPhotos : MonoBehaviour
{

    [SerializeField] private GameObject polaroidPrefab;
    [SerializeField] private Transform polaroidParent;
    private SpriteRenderer spriteRenderer;
    private GameObject createdPolaroid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { 
            DisplayPhoto();
        }
    }

void DisplayPhoto()
{
    float spawnDistance = 0.6f; // Base distance in front of the camera
    float sideDistanceIncrement = -0.1f; // Increment for side offset
    float depthOffsetIncrement = -0.1f; // Increment for depth
    int photoCount = PhotoManager.GetPhotoCount();

    Camera playerCamera = Camera.main;

    if (playerCamera == null)
    {
        Debug.LogError("No main camera found!");
        return;
    }

    for (int i = 0; i < photoCount; i++)
    {
        Sprite sprite = PhotoManager.GetPhoto(i);

        // Calculate base position in front of the camera
        Vector3 basePosition = playerCamera.transform.position + playerCamera.transform.forward * spawnDistance;

        // Add offsets for side positioning and depth stacking
        Vector3 sideOffset = playerCamera.transform.right * (i * sideDistanceIncrement); // Offset each photo to the side
        Vector3 depthOffset = -playerCamera.transform.forward * (i * depthOffsetIncrement); // Offset each photo slightly behind the last one

        // Calculate final spawn position
        Vector3 spawnPosition = basePosition + sideOffset + depthOffset;

        // Instantiate the polaroid prefab at the calculated position
        GameObject spawnedPolaroid = Instantiate(polaroidPrefab, spawnPosition, Quaternion.identity);

        // Set the polaroid as a child of the camera to make it follow
        spawnedPolaroid.transform.SetParent(playerCamera.transform);

        // Adjust local rotation to ensure proper alignment
        spawnedPolaroid.transform.localRotation = Quaternion.identity; // Neutral rotation
        spawnedPolaroid.transform.LookAt(playerCamera.transform); // Make it face the camera
        spawnedPolaroid.transform.Rotate(270, 90, 90); // Flip to ensure the front faces the player

        // Assign the captured sprite to the polaroid's SpriteRenderer
        SpriteRenderer spriteRenderer = spawnedPolaroid.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
}