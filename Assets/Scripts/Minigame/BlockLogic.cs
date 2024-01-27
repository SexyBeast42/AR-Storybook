using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        var colliders = Physics.OverlapBox(transform.position, transform.localScale/2);
        foreach (var collider in colliders)
        {
            //Debug.Log($"DebugLog: {name} has been hit by {collider}");
            
            if (collider.GetComponent<SphereLogic>() == null) continue;
            
            //Debug.Log($"DebugLog: Found a sphere, triggering sphere");
            collider.GetComponent<SphereLogic>().HasBeenTriggered();
        }
    }
}
