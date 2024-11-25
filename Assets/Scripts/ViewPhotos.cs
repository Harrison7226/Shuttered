using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPhotos : MonoBehaviour
{

    [SerializeField] private GameObject polaroidPrefab;
    [SerializeField] private Transform polaroidParent;
    private SpriteRenderer spriteRenderer;
    private GameObject createdPolaroid;

    private int currentIndex = 0; // Index of the currently viewed photo


    private List<GameObject> spawnedPolaroids = new List<GameObject>(); // Track spawned polaroids

    private bool isTransitioning = false; // Prevent multiple transitions at the same time

    private bool galleryEnabled = false;

    float spawnDistance = 0.6f; // Base distance in front of the camera
    float sideDistanceIncrement = -0.05f; // Increment for side offset
    float depthOffsetIncrement = 0.05f; // Increment for depth
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && galleryEnabled == false) {
            currentIndex = PhotoManager.GetPhotoCount() - 1;
            galleryEnabled = true;
            DisplayPhotos(currentIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && galleryEnabled == true) {
            galleryEnabled = false;
            DeleteAllPhotos();
        }
        if (Input.GetKeyDown(KeyCode.O) && !isTransitioning)
        {
            ScrollToNextPhoto(); // Scroll to the next photo
        }
        if (Input.GetKeyDown(KeyCode.P) && !isTransitioning)
        {
            ScrollToPreviousPhoto(); // Scroll to the previous photo
        }

    }

void DisplayPhotos(int index)
{
    int photoCount = PhotoManager.GetPhotoCount();

    Camera playerCamera = Camera.main;

    for (int i = 0; i < photoCount; i++)
    {
        // Retrieve the sprite for the current photo index
        Sprite sprite = PhotoManager.GetPhoto(index);

        // Calculate base position in front of the camera
        Vector3 basePosition = playerCamera.transform.position + playerCamera.transform.forward * spawnDistance;

        // Add offsets for side positioning and depth stacking
        Vector3 sideOffset = playerCamera.transform.right * (i * sideDistanceIncrement); // Offset each photo to the side
        Vector3 depthOffset = playerCamera.transform.forward * (i * depthOffsetIncrement); // Offset each photo slightly behind the last one

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

        // Assign the unique sprite to the polaroid's SpriteRenderer
        SpriteRenderer spriteRenderer = spawnedPolaroid.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        MeshRenderer meshRenderer = spawnedPolaroid.GetComponent<MeshRenderer>();
        Material material = meshRenderer.material;
        Color color = material.color; // Get the current color
        color.a = 1f - (i * 0.05f); // Set the alpha
        material.color = color; // Apply the modified color

        if (spriteRenderer != null)
        {
            Color spriteColor = spriteRenderer.color; // Get the current color
            spriteColor.a = 1f - (i * 0.05f); // Set the alpha (0 = fully transparent, 1 = fully opaque)
            spriteRenderer.color = spriteColor; // Apply the modified color
            spriteRenderer.sprite = sprite;
        }

        // Add the spawned polaroid to the list for later deletion
        spawnedPolaroids.Add(spawnedPolaroid);
    }
}
    void DeleteAllPhotos()
    {
        // Iterate through the list and destroy each polaroid
        foreach (GameObject polaroid in spawnedPolaroids)
        {
            Destroy(polaroid);
        }

        // Clear the list
        spawnedPolaroids.Clear();
    }
    void ScrollToNextPhoto() {
        if (currentIndex < spawnedPolaroids.Count - 1) {
            currentIndex++;
            DeleteAllPhotos();
            DisplayPhotos(currentIndex);

        }
    }

    void ScrollToPreviousPhoto() {
        if (currentIndex > 0) {
            currentIndex--;
            DeleteAllPhotos();
            DisplayPhotos(currentIndex);
        }
    }
}
