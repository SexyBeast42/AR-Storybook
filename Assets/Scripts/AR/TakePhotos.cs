using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class TakePhotos : MonoBehaviour
{
    private const string PictureDir = "MinigamePhotos";
    private string _fullPath;

    public PictureLibrary pictureLibrary;

    private void Awake()
    {
        //pictureNames = GetComponent<PictureNames>();
        
        _fullPath = Path.Combine(Application.persistentDataPath, PictureDir);
        if (!Directory.Exists(_fullPath))
        {
            Debug.Log($"DebugLog: Directory has been created");
            Directory.CreateDirectory(_fullPath);
        }
        
        Debug.Log($"DebugLog: directory saved onto: " + _fullPath);
    }

    public void TakePhoto()
    {
        Debug.Log($"DebugLog: Called TakePhoto");
        StartCoroutine(TakeAPhoto());
    }

    IEnumerator TakeAPhoto()
    {
        yield return new WaitForEndOfFrame();
        Camera camera = Camera.main;
        int width = Screen.width;
        int height = Screen.height;

        RenderTexture rt = new RenderTexture(width, height, 24);
        camera.targetTexture = rt;

        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;
        
        camera.Render();

        Texture2D image = new Texture2D(width, height);
        image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        image.Apply();
        Debug.Log($"DebugLog: Photo was taken");

        camera.targetTexture = null;

        RenderTexture.active = currentRT;

        Debug.Log($"DebugLog: Saving images to internal storage");
        byte[] bytes = image.EncodeToPNG();
        string fileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = Path.Combine(_fullPath, fileName);
        Debug.Log($"DebugLog: {filePath} and {fileName} was successfully created");
        
        File.WriteAllBytes(filePath, bytes);
        pictureLibrary.SavePhoto(filePath, fileName);
        //StorePicture(bytes, fileName);
        
        Debug.Log( "DebugLog: Saved image in " + filePath + " as " + fileName);
        
        Destroy(rt);
        Destroy(image);
        
        pictureLibrary.ChangeSceneOnEnoughPictures();
    }

    private void StorePicture(byte[] photo, string name)
    {
        using (var outStream = new FileStream(Path.Combine(_fullPath, name), FileMode.CreateNew, FileAccess.Write))
        {
            outStream.Write(photo, 0, photo.Length);
        }
    }
}
