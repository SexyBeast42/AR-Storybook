using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TakePhotosUI : MonoBehaviour
{
    private PictureLibrary _pictureLibrary;
    public TMP_Text buttonText;
    
    private void Start()
    {
        _pictureLibrary = FindObjectOfType<PictureLibrary>();
        _pictureLibrary.onSavePhoto.AddListener(UpdateButtonText);

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        Debug.Log($"DebugLog: current tag is {_pictureLibrary.GetPhotoTag()}");
        
        buttonText.text = _pictureLibrary.GetPhotoTag();
    }
}
