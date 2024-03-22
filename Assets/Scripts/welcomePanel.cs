using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Always Shows a Welcome Panel when Application Starts

public class welcomePanel : MonoBehaviour
{
   static bool  isFirst=true;
    void Start()
    {
        if (!isFirst)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        isFirst = false;
    }
}
