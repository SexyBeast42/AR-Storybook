using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Page ", menuName = "Page Information")]
public class PageInformation: ScriptableObject
{
    public string pageLine;
    public GameObject pageModelScene;
    public GameObject miniGame;
    public bool hasMiniGame;
    public bool hasFinishedMiniGame;
}
