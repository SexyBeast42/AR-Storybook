using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VisualNovelController : MonoBehaviour
{
    public List<PageInformation> pages = new List<PageInformation>();
    
    public GameObject playArea;
    public GameObject miniGameObj;
    private Button _miniGameButton;
    private Renderer _playAreaRenderer;
    public TMP_Text visualNovelText;

    private int _currentPageNumber = 0;
    private GameObject _displayedModel, _miniGameToSpawn;
    public bool isPlayingMiniGame = false;

    void Start()
    {
        _playAreaRenderer = playArea.GetComponent<Renderer>();
        _miniGameButton = miniGameObj.GetComponentInChildren<Button>();
        Debug.Log($"DebugLog: found mini game button: {_miniGameButton != null}");
        
        miniGameObj.SetActive(false);
    }

    void Update()
    {
        DisplayText();
        DisplayModel();
        CheckForMiniGame();
    }
    
    private void DisplayText()
    {
        visualNovelText.text = pages[_currentPageNumber].pageLine;
    }

    private void DisplayModel()
    {
        if (_displayedModel != null)
        {
            Destroy(_displayedModel);
        }
        
        if (pages[_currentPageNumber].hasMiniGame) return;

        _displayedModel = Instantiate(pages[_currentPageNumber].pageModelScene, _playAreaRenderer.bounds.center, playArea.transform.rotation);
    }

    private void CheckForMiniGame()
    {
        //miniGameObj.SetActive(false);
        
        if (!pages[_currentPageNumber].hasMiniGame || isPlayingMiniGame) return;

        if (pages[_currentPageNumber].hasFinishedMiniGame)
        {
            if (_displayedModel != null)
            {
                Destroy(_displayedModel);
            }
            
            _displayedModel = Instantiate(pages[_currentPageNumber].pageModelScene, _playAreaRenderer.bounds.center,
                playArea.transform.rotation);
            return;
        }
        
        if (_miniGameToSpawn == null && !pages[_currentPageNumber].hasFinishedMiniGame)
        {
            Debug.Log($"DebugLog: Setting minigame to {pages[_currentPageNumber].miniGame}");
            _miniGameToSpawn = pages[_currentPageNumber].miniGame;
            miniGameObj.SetActive(true);
        }
    }

    public void SpawnMiniGame()
    {
        miniGameObj.SetActive(false);
        isPlayingMiniGame = true;
        Debug.Log($"DebugLog: Deactivating minigameobj: {miniGameObj.activeSelf}");
        
        Instantiate(_miniGameToSpawn, _playAreaRenderer.bounds.center, playArea.transform.rotation);
        _miniGameToSpawn = null;
    }

    public void NextPage()
    {
        if (_currentPageNumber == pages.Count - 1 || isPlayingMiniGame) return;
        if (miniGameObj.activeSelf)
        {
            miniGameObj.SetActive(false);
            _miniGameToSpawn = null;
        }
        
        
        _currentPageNumber++;
    }

    public void LastPage()
    {
        if (_currentPageNumber == 0 || isPlayingMiniGame) return;
        if (miniGameObj.activeSelf)
        {
            miniGameObj.SetActive(false);
            _miniGameToSpawn = null;
        }

        _currentPageNumber--;
    }
}
