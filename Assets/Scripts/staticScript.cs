using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class AnimatedStaticNoise : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public float noiseFrequency = 0.1f;
    public float animationSpeed = 0.1f;

    private Texture2D noiseTexture;
    private RawImage rawImage;
    private Color[] pixels;
    private float timeOffset;

    void Start()
    {
        // Initialize the texture and RawImage component
        noiseTexture = new Texture2D(textureWidth, textureHeight);
        rawImage = GetComponent<RawImage>();
        rawImage.texture = noiseTexture;
        pixels = new Color[textureWidth * textureHeight];
    }

    void Update()
    {
        // Update the noise texture every frame
        timeOffset += animationSpeed * Time.deltaTime;
        GenerateNoiseTexture();
        noiseTexture.SetPixels(pixels);
        noiseTexture.Apply();
    }

    void GenerateNoiseTexture()
    {
        // Generate random noise for each pixel
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                // Use Perlin noise for a smoother static effect
                float sample = Mathf.PerlinNoise(x * noiseFrequency + timeOffset, y * noiseFrequency + timeOffset);
                pixels[y * textureWidth + x] = new Color(sample, sample, sample);
            }
        }
    }
}
