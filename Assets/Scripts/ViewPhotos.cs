using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPhotos : MonoBehaviour
{

    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip soundEffect;   // Reference to the sound effect
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
        KeyCode openGalleryKey = KeyCode.R;
        KeyCode moveBackwardKey = KeyCode.Q;
        KeyCode moveForwardKey = KeyCode.E;
        if (GameObject.Find("KeybindSwitcher") != null) {
            openGalleryKey = KeybindManager.Instance.GetKeyForAction("Open Gallery");
            moveBackwardKey = KeybindManager.Instance.GetKeyForAction("Move Backward");
            moveForwardKey = KeybindManager.Instance.GetKeyForAction("Move Forward");
        }
        if (Input.GetKeyDown(openGalleryKey)) {
            if (galleryEnabled) {
                    galleryEnabled = false;
                    DeleteAllPhotos();
            }
            else {
                galleryEnabled = true;
                DisplayPhotos(currentIndex);
            }
        }
        if (Input.GetKeyDown(moveBackwardKey) && !isTransitioning)
        {
            ScrollToNextPhoto(); // Scroll to the next photo
        }
        if (Input.GetKeyDown(moveForwardKey) && !isTransitioning)
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
            audioSource.PlayOneShot(soundEffect);
            

        }
    }

    void ScrollToPreviousPhoto() {
        if (currentIndex > 0) {
            currentIndex--;
            DeleteAllPhotos();
            DisplayPhotos(currentIndex);
            audioSource.PlayOneShot(soundEffect);
        }
    }
}
