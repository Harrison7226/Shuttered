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
        KeyCode key = KeybindManager.Instance.GetKeyForAction("Open Gallery");
        if (Input.GetKeyDown(key)){
            if (galleryEnabled) {
                    galleryEnabled = false;
                    DeleteAllPhotos();
            }
            else {
                galleryEnabled = true;
                DisplayPhotos(currentIndex);
            }
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

void DisplayPhotos(int index) {
    int photoCount = PhotoManager.GetPhotoCount();

    Camera playerCamera = Camera.main;

    for (int i = 0; i < index; i++) {
        Sprite sprite = PhotoManager.GetPhoto(i);

        Vector3 basePosition = playerCamera.transform.position + playerCamera.transform.forward * spawnDistance;

        Vector3 sideOffset = playerCamera.transform.right * ((index - i)* (-1 * sideDistanceIncrement)); 
        Vector3 depthOffset = playerCamera.transform.forward * ((index - i) * depthOffsetIncrement);

        Vector3 spawnPosition = basePosition + sideOffset + depthOffset;

        GameObject spawnedPolaroid = Instantiate(polaroidPrefab, spawnPosition, Quaternion.identity);

        spawnedPolaroid.transform.SetParent(playerCamera.transform);

        spawnedPolaroid.transform.localRotation = Quaternion.identity;
        spawnedPolaroid.transform.LookAt(playerCamera.transform); 
        spawnedPolaroid.transform.Rotate(270, 90, 90); 

        SpriteRenderer spriteRenderer = spawnedPolaroid.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color spriteColor = spriteRenderer.color; 
            spriteRenderer.color = spriteColor; 
            spriteRenderer.sprite = sprite;
        }
        
        spawnedPolaroids.Add(spawnedPolaroid);
    }

    for (int i = index; i < photoCount; i++) {
        Sprite sprite = PhotoManager.GetPhoto(i);

        Vector3 basePosition = playerCamera.transform.position + playerCamera.transform.forward * spawnDistance;

        Vector3 sideOffset = playerCamera.transform.right * ((i - index) * sideDistanceIncrement); 
        Vector3 depthOffset = playerCamera.transform.forward * ((i - index) * depthOffsetIncrement);

        Vector3 spawnPosition = basePosition + sideOffset + depthOffset;

        GameObject spawnedPolaroid = Instantiate(polaroidPrefab, spawnPosition, Quaternion.identity);

        spawnedPolaroid.transform.SetParent(playerCamera.transform);

        spawnedPolaroid.transform.localRotation = Quaternion.identity;
        spawnedPolaroid.transform.LookAt(playerCamera.transform); 
        spawnedPolaroid.transform.Rotate(270, 90, 90); 

        SpriteRenderer spriteRenderer = spawnedPolaroid.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color spriteColor = spriteRenderer.color; 
            spriteRenderer.color = spriteColor; 
            spriteRenderer.sprite = sprite;
        }
        
        spawnedPolaroids.Add(spawnedPolaroid);
    }
    
}
    void DeleteAllPhotos()
    {
        foreach (GameObject polaroid in spawnedPolaroids)
        {
            Destroy(polaroid);
        }
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
