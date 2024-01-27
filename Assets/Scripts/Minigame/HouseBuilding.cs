using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class HouseBuilding : MonoBehaviour
{
    public PageInformation pageInfo;
    
    //public GameObject stickHouse;
    public List<GameObject> sphereObj = new List<GameObject>();
    private List<SphereLogic> _sphereLogic = new List<SphereLogic>();
    private int _sphereProgress = 0;
    private bool _hasFinished = false;
    
    private void Start()
    {
        //stickHouse.SetActive(false);

        foreach (var sphere in sphereObj)
        {
            _sphereLogic.Add(sphere.GetComponent<SphereLogic>());
        }
    }

    private void Update()
    {
        CheckProgress();
        MiniGameFinish();
    }

    private void CheckProgress()
    {
        if (_sphereProgress >= sphereObj.Count)
        {
            _hasFinished = true;
            return;
        }
        
        foreach (var sphere in _sphereLogic)
        {
            if (sphere.hasTriggered)
            {
                Debug.Log($"DebugLog: {sphere.name} has been triggered, adding one to the counter");
                _sphereProgress++;
                sphere.gameObject.SetActive(false);
                _sphereLogic.Remove(sphere);
            }
        }
    }

    private void MiniGameFinish()
    {
        if (!_hasFinished) return;

        //stickHouse.SetActive(true);

        pageInfo.hasFinishedMiniGame = true;
        Debug.Log($"DebugLog: finished minigame and set pageinfo: {pageInfo.hasFinishedMiniGame}");
        FindObjectOfType<VisualNovelController>().isPlayingMiniGame = false;
        gameObject.SetActive(false);
    }
}
