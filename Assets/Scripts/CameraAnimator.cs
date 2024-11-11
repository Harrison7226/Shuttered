using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject cameraOverlay, shutter;
    [SerializeField] private PostProcessVolume volume;

    [Header("Audio")]
    [SerializeField] private AudioSource developSFX;
    [SerializeField] private AudioSource shutterSFX;

    [Header("Zoom Effect")]
    [SerializeField] private int zoom = 20;
    [SerializeField] private int normal = 60;
    [SerializeField] private float smooth = 5;

    private Vignette vignette;
    private bool canTakePhoto, printing;

    public bool CanTakePhoto
    {
        get => canTakePhoto;
        set => canTakePhoto = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        CanTakePhoto = false;
        vignette = volume.profile.GetSetting<Vignette>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Check if camera is already taking photo
            if (animator.GetBool("TakingPhoto") && !printing)
            {
                animator.SetBool("TakingPhoto", false);
                SetCameraState(false);
                CanTakePhoto = false;
            }
            // Check if camera is not taking photo
            else if (!animator.GetBool("TakingPhoto"))
            {
                animator.SetBool("TakingPhoto", true);
            }
        }

        // Zoom effect when can take photo
        if (Input.GetKey(KeyCode.Z) && canTakePhoto)
        {
            animator.enabled = false; // Disable animator to allow changing field of view
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
        }
        else
        {
            animator.enabled = true;
        }

        // Check to remove camera overlay if not taking photo
        // Fixes bug where camera overlay is still active after leaving camera mode
        if (!animator.GetBool("TakingPhoto"))
        {
            SetCameraState(false);
            CanTakePhoto = false;
        }
    }

    // Called by 'TakePhoto' animation
    public void OnCameraActive()
    {
        SetCameraState(true);
        CanTakePhoto = true;
    }

    // Called by 'PrintPhoto' animation
    public void OnDevelop()
    {
        developSFX.Play();
    }

    // Called by 'PrintPhoto' animation
    public void OnPrint()
    {
        printing = false;
        animator.SetBool("TakingPhoto", false);
    }

    // Set camera overlay and vignette state
    public void SetCameraState(bool state)
    {
        cameraOverlay.SetActive(state);
        vignette.enabled.value = state;
    }

    // Take photo
    public void TakePhoto()
    {
        StartCoroutine(ShowShutter());
    }

    // Show shutter effect
    private IEnumerator ShowShutter()
    {
        printing = true;
        shutter.SetActive(true);
        SetCameraState(true);
    
        yield return new WaitForSeconds(0.25f);

        shutter.SetActive(false);
        shutterSFX.Play();

        yield return new WaitForSeconds(0.25f);

        SetCameraState(false);
        animator.SetTrigger("PhotoTaken");
    }
}
