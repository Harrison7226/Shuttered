using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(ColorChangeRenderer), PostProcessEvent.AfterStack, "Custom/PostProcessGlitchShader")]
public sealed class ColorChangeEffect : PostProcessEffectSettings
{
    [Tooltip("Tint Color")]
    public ColorParameter tintColor = new ColorParameter { value = Color.red };
}

public sealed class ColorChangeRenderer : PostProcessEffectRenderer<ColorChangeEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        Debug.Log("Applying ColorChange Effect");
        var sheet = context.propertySheets.Get(Shader.Find("Custom/PostProcessGlitchShader"));
        if (sheet == null)
        {
            Debug.LogError("Shader not found: Custom/PostProcessGlitchShader");
            return;
        }

        sheet.properties.SetColor("_TintColor", settings.tintColor);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
