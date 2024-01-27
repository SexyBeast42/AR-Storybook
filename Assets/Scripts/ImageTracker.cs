using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    public GameObject prefab;
    public PictureLibrary pictureLibrary;
    private ARTrackedImageManager _trackedImageManager;
    private MutableRuntimeReferenceImageLibrary _mutableTrackedImageManager;
    public UnityEvent subSystemStarted;
    private Dictionary<ARTrackedImage, GameObject> _arObjects = new Dictionary<ARTrackedImage, GameObject>();

    void Start()
    {
        _trackedImageManager = gameObject.AddComponent<ARTrackedImageManager>();
        subSystemStarted = new UnityEvent();
        
        if (_trackedImageManager != null)
        {
            Debug.Log($"DebugLog: TrackedImageManager was created");
        }

        pictureLibrary = FindObjectOfType<PictureLibrary>();

        if (pictureLibrary != null)
        {
            Debug.Log($"DebugLog: Picture library was found");
        }
        
        subSystemStarted.AddListener(() => SetupARListeners());
        
        StartLibrary();
    }

    private void StartLibrary()
    {
        Debug.Log($"DebugLog: Starting the library");
        
        _trackedImageManager.CreateRuntimeLibrary();

        RuntimeReferenceImageLibrary runtimeLibrary = _trackedImageManager.CreateRuntimeLibrary();

        if (runtimeLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            // mutableLibrary = _trackedImageManager.CreateRuntimeLibrary() as MutableRuntimeReferenceImageLibrary;

            Debug.Log($"DebugLog: Created mutable library, now trying to add all images to mutable reference image library");
            
            StartCoroutine(AddAllImagesToMutableReferenceImageLibraryAR(mutableLibrary));
        }
    }

    private IEnumerator AddAllImagesToMutableReferenceImageLibraryAR(MutableRuntimeReferenceImageLibrary mutableLibrary)
    {
        yield return null;

        AddReferenceImageJobState job;

        if (!System.IO.File.Exists(pictureLibrary.photos[0].filePath))
        {
            Debug.Log($"DebugLog: The file path for the first photo doesnt exist");
            yield break;
        }

        foreach (var photo in pictureLibrary.photos)
        {
            Debug.Log($"DebugLog: Loading image: {photo.fileName} from  path: {photo.filePath}");
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(File.ReadAllBytes(Path.Combine(Application.persistentDataPath, photo.filePath)));

            if (!texture2D.isReadable)
            {
                Debug.Log($"DebugLog: {texture2D.name}, for {photo.fileTag} isn't readable");
            }
            
            Debug.Log($"DebugLog: Loaded image: {photo.fileName} with tag: {photo.fileTag} from  path: {photo.filePath}");
            job = mutableLibrary.ScheduleAddImageWithValidationJob(texture2D, photo.fileName, 0.1f);
            
            yield return new WaitUntil(() => job.jobHandle.IsCompleted);
            
            Debug.Log($"DebugLog: Job status: {job.status}");
        }

        _trackedImageManager.referenceLibrary = mutableLibrary;
        _trackedImageManager.enabled = true;
        Debug.Log($"DebugLog: Image manager library is set up and enabled");

        _trackedImageManager.requestedMaxNumberOfMovingImages = pictureLibrary.photos.Count;
        _trackedImageManager.subsystem.Start();
        Debug.Log($"DebugLog: Subsystem running: {_trackedImageManager.subsystem.running}");
        
        subSystemStarted.Invoke();
    }

    private void SetupARListeners()
    {
        _trackedImageManager.trackedImagesChanged += TrackImage;
        Debug.Log($"DebugLog: Setup listeners are done");
    }

    private void TrackImage(ARTrackedImagesChangedEventArgs obj)
    {
        //Debug.Log("DebugLog: Tracked changed");

        foreach (var i in obj.added)
        {
            //Debug.Log($"DebugLog: Added: {i.referenceImage.name}");
            var o = SpawnObject(i);
            _arObjects.Add(i, o);
        }

        foreach (var i in obj.updated)
        {
            //Debug.Log($"DebugLog: updated: {i.referenceImage.name}");
            UpdateImage(i);
        }

        foreach (var i in obj.removed)
        {
            //Debug.Log($"DebugLog: removed: {_arObjects[i].name}");
            _arObjects.Remove(i);
            Destroy(_arObjects[i]);
        }
    }

    private GameObject SpawnObject(ARTrackedImage image)
    {
        // GameObject obj;
        //
        // for (int i = 0; i < pictureLibrary.photos.Count(); i++)
        // {
        //     PhotoFile photo = pictureLibrary.photos[i];
        //     
        //     Texture2D texture2D = new Texture2D(1, 1);
        //     texture2D.LoadImage(File.ReadAllBytes(Path.Combine(Application.persistentDataPath, photo.filePath)));
        //
        //     if (image == texture2D)
        //     {
        //         
        //     }
        // }
        
        return Instantiate(prefab, image.transform.position, image.transform.rotation);
    }

    private void UpdateImage(ARTrackedImage image)
    {
        //Debug.Log($"DebugLog: Moving gameobject attached to {image.name}");
        
        _arObjects[image].transform.position = image.transform.position;
        _arObjects[image].transform.rotation = image.transform.rotation;
    }
}
