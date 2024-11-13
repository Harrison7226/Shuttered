using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText;
    public Color normalGlowColor = Color.clear;
    public Color hoverGlowColor = Color.white;
    public float normalGlowPower = 0.0f;
    public float hoverGlowPower = 1.5f;
    public float normalOpacity = 0.8f;
    public float hoverOpacity = 1.0f;

    private Material textMaterial;

    void Start()
    {
        // Get the TextMeshPro material
        textMaterial = buttonText.fontMaterial;
        // Set initial glow color, power, and opacity
        textMaterial.SetColor("_GlowColor", normalGlowColor);
        textMaterial.SetFloat("_GlowPower", normalGlowPower);
        SetTextOpacity(normalOpacity);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change glow color, power, and opacity on hover
        textMaterial.SetColor("_GlowColor", hoverGlowColor);
        textMaterial.SetFloat("_GlowPower", hoverGlowPower);
        SetTextOpacity(hoverOpacity);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset glow color, power, and opacity when hover ends
        textMaterial.SetColor("_GlowColor", normalGlowColor);
        textMaterial.SetFloat("_GlowPower", normalGlowPower);
        SetTextOpacity(normalOpacity);
    }

    private void SetTextOpacity(float opacity)
    {
        // Set the opacity of the text (affects the face color alpha)
        Color faceColor = textMaterial.GetColor("_FaceColor");
        faceColor.a = opacity;
        textMaterial.SetColor("_FaceColor", faceColor);
    }
}
