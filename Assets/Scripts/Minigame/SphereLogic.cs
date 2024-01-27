using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereLogic : MonoBehaviour
{
    public bool hasTriggered = false;

    public void HasBeenTriggered()
    {
        hasTriggered = true;
    }
}
