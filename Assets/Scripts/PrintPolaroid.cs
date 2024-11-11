using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintPolaroid : MonoBehaviour
{
    [SerializeField] private int resolutionWidth = 1920;
    [SerializeField] private int resolutionHeight = 1080;
    [SerializeField] private GameObject polaroidPrefab;
    [SerializeField] private Transform polaroidParent;
    [SerializeField] private CameraAnimator cameraAnimator;

    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;

    private SpriteRenderer spriteRenderer;
    private GameObject createdPolaroid;

    public void LateUpdate()
    {
        if (cameraAnimator.CanTakePhoto && Input.GetMouseButtonDown(0))
        {
            cameraAnimator.CanTakePhoto = false;
            cameraAnimator.SetCameraState(false);
            
            StartCoroutine(CameraFlashEffect());
            
            // Create and set up render texture
            RenderTexture renderTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);
            Camera.main.targetTexture = renderTexture;

            // Create texture to store screenshot
            Texture2D screenShot = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);

            // Render camera to texture
            Camera.main.Render();
            RenderTexture.active = renderTexture;

            // Read pixels from render texture
            screenShot.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
            screenShot.Apply();

            // Clean up
            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            // Create sprite from screenshot
            Sprite sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f), 1000f);
            
            // Destroy previous polaroid
            if (createdPolaroid != null)
            {
                Destroy(createdPolaroid);
            }

            // Create new polaroid 
            createdPolaroid = Instantiate(polaroidPrefab, polaroidParent);
            spriteRenderer = createdPolaroid.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

            cameraAnimator.TakePhoto();
        }
    }

    // Flash effect when taking photo
    public IEnumerator CameraFlashEffect()
    {
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }
}
