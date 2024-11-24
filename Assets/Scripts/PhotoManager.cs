using System.Collections.Generic;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{
    // Public static list to store photos
    public static List<Sprite> PhotoGallery = new List<Sprite>();

    // Optionally, add helper methods to manage the gallery
    public static void AddPhoto(Sprite photo)
    {
        PhotoGallery.Add(photo);
    }
    public static Sprite GetPhoto(int index)
    {
        if (index >= 0 && index < PhotoGallery.Count)
        {
            return PhotoGallery[index];
        }
        return null;
    }

    public static int GetPhotoCount()
    {
        return PhotoGallery.Count;
    }
}
