using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    public static UIManager Instance;
    [SerializeField] TextMeshProUGUI titleText, descriptionText;


    private void Awake()
    {
        Instance = this;
    }


    public void SetDetails(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;

    }

}
    
  
