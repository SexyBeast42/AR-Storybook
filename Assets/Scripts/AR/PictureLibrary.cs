using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public class PhotoFile
{
    public string filePath, fileName, fileTag;
    public GameObject fileObj;
}

public class PictureLibrary : MonoBehaviour
{
    public List<PhotoFile> photos = new List<PhotoFile>();
    public int currentPicture = 0;
    public string nextScene = "DisplayObject";

    public UnityEvent onSavePhoto = new UnityEvent();
    
    private void Start()
    {
        Debug.Log($"DebugLog: {gameObject.name} is in the scene {SceneManager.GetActiveScene().name}");
        Debug.Log($"DebugLog: total pictures is: {photos.Count}");
        
        onSavePhoto.AddListener(UpdateCurrentPicture);
        
        DontDestroyOnLoad(gameObject);
    }

    public void SavePhoto(string filePath, string fileName)
    {
        photos[currentPicture].filePath = filePath;
        photos[currentPicture].fileName = fileName;
        
        onSavePhoto?.Invoke();

        // PhotoFile savePhotoFile = new PhotoFile();
        // savePhotoFile.filePath = filePath;
        // savePhotoFile.fileName = fileName;
        //
        // photos.Add(savePhotoFile);
    }
    
    private void UpdateCurrentPicture()
    {
        currentPicture++;
    }

    public string GetPhotoTag()
    {
        return photos[currentPicture].fileTag;
    }
    
    public void ChangeSceneOnEnoughPictures()
    {
        Debug.Log($"DebugLog: Comparing currentpicture: {currentPicture} to photos.count: {photos.Count}");
        
        if (currentPicture >= photos.Count)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}