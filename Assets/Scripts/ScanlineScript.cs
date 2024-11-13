using UnityEngine;
using TMPro;

[ExecuteAlways]
public class TextMeshProHorizontalWarp : MonoBehaviour
{
    public float warpStrength = 5.0f; // Strength of the warp effect
    public float frequency = 2.0f; // Frequency of the warp pattern
    public float speed = 2.0f; // Speed of the warp animation

    private TMP_Text textMeshPro;
    private Vector3[] vertices;

    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro component not found!");
            enabled = false;
            return;
        }
    }

    void LateUpdate()
    {
        // Force the text mesh to update
        textMeshPro.ForceMeshUpdate();

        // Get the text info and mesh data
        TMP_TextInfo textInfo = textMeshPro.textInfo;

        // Iterate through each material mesh (TextMeshPro can use multiple materials)
        for (int m = 0; m < textInfo.meshInfo.Length; m++)
        {
            vertices = textInfo.meshInfo[m].vertices;

            // Iterate through all characters
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                // Get the midpoint of the character
                float midpointX = (charInfo.bottomLeft.x + charInfo.topRight.x) / 2f;

                // Iterate through the 4 vertices of the character quad
                for (int j = 0; j < 4; j++)
                {
                    int vertexIndex = charInfo.vertexIndex + j;

                    // Calculate the distance from the midpoint
                    float distanceFromMidpoint = vertices[vertexIndex].x - midpointX;

                    // Apply a horizontal warp effect based on the distance from the midpoint
                    float warpOffset = Mathf.Sin(distanceFromMidpoint * frequency + Time.time * speed) * warpStrength;

                    // Shift the vertex horizontally based on the warp effect
                    vertices[vertexIndex].x += warpOffset;
                }
            }
        }

        // Update the mesh with the modified vertices
        textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
