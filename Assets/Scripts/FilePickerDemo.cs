using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Android;

public class FilePickerDemo : MonoBehaviour
{
    public RawImage displayImage; // Assign a RawImage from the UI to preview the selected image

    void Start()
    {
        // Request permission for storage access
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    public void PickImage()
    {
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path != null)
            {
                Debug.Log("Selected file path: " + path);
                StartCoroutine(LoadImage(path)); // Load and display image
            }
        }, new string[] { "image/*" }); // Filter only image files

        Debug.Log("Permission result: " + permission);
    }

    IEnumerator LoadImage(string path)
    {
        // Read the file as bytes
        byte[] bytes = File.ReadAllBytes(path);

        // Create a texture from the bytes
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        // Display image in UI
        displayImage.texture = texture;

        // Save to persistent data path
        string persistentPath = Path.Combine(Application.persistentDataPath, Path.GetFileName(path));
        File.WriteAllBytes(persistentPath, bytes);

        Debug.Log("Image saved at: " + persistentPath);
        yield return null;
    }
}
