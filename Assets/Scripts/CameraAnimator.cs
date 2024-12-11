using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraAnimator : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    private bool printing;
    private bool canAimCamera = true;
    public bool canTakePhoto;

    [Header("Audio")]
    [SerializeField] private AudioSource developSFX;
    [SerializeField] private AudioSource shutterSFX;

    [Header("Camera Overlay")]
    [SerializeField] private GameObject cameraOverlay, shutter;
    [SerializeField] private PostProcessVolume volume;
    private Vignette vignette;

    [Header("Zoom Effect")]
    [SerializeField] private int zoom = 20;
    [SerializeField] private float smooth = 5;

    // Start is called before the first frame update
    void Start()
    {
        vignette = volume.profile.GetSetting<Vignette>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Check if camera is already taking photo
            if (animator.GetBool("TakingPhoto") && !printing && canTakePhoto)
            {
                animator.SetBool("TakingPhoto", false);
                SetCameraState(false);
                canTakePhoto = false;
            }
            // Check if camera is not taking photo
            else if (!animator.GetBool("TakingPhoto") && canAimCamera)
            {
                animator.SetBool("TakingPhoto", true);
                canAimCamera = false;
            }
        }

        // Zoom effect when can take photo

        KeyCode zoomKey = KeyCode.Z;
        if (GameObject.Find("KeybindSwitcher") != null) {
            zoomKey = KeybindManager.Instance.GetKeyForAction("Zoom");
        }

        if (Input.GetKey(zoomKey) && canTakePhoto)
        {
            animator.enabled = false; // Disable animator to allow changing field of view
            GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, zoom, Time.deltaTime * smooth);
        }
        else
        {
            animator.enabled = true;
        }
    }

    // Called by 'TakePhoto' animation
    public void OnCameraActive()
    {
        SetCameraState(true);
        canTakePhoto = true;
    }

    // Called by 'BackFromTake' animation
    public void OnCameraDeactive()
    {
        canAimCamera = true;
    }

    // Called by 'PrintPhoto' animation
    public void OnDevelop()
    {
        developSFX.Play();
    }

    // Called by 'PrintPhoto' animation
    public void OnPrint()
    {
        animator.SetBool("TakingPhoto", false);
        canAimCamera = true;
        printing = false;
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
