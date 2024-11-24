using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Material videoMaterial; // Assign the material used by the video
    public float hoverGlowIntensity = 5f; // Glow intensity when hovering
    private float defaultGlowIntensity; // Original glow intensity

    void Start()
    {
        // Get the default glow intensity from the material
        if (videoMaterial != null && videoMaterial.HasProperty("_GlowIntensity"))
        {
            defaultGlowIntensity = videoMaterial.GetFloat("_GlowIntensity");
        }
    }

    // Triggered when the mouse pointer enters the button
public void OnPointerEnter(PointerEventData eventData)
{
    Debug.Log("Pointer entered the button");
    if (videoMaterial != null && videoMaterial.HasProperty("_GlowIntensity"))
    {
        videoMaterial.SetFloat("_GlowIntensity", hoverGlowIntensity);
    }
}

public void OnPointerExit(PointerEventData eventData)
{
    Debug.Log("Pointer exited the button");
    if (videoMaterial != null && videoMaterial.HasProperty("_GlowIntensity"))
    {
        videoMaterial.SetFloat("_GlowIntensity", defaultGlowIntensity);
    }
}

}
