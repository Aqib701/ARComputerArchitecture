using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// ****  Deals with Enabling and Disabling Objects On Button Clicks ****



public class animationByPartsManager : MonoBehaviour
{

    public SelectableObject[] PcComponents;
    public Transform pointerObject;
    void Start()
    {
        enableObject(0); // Enable First Object by Default
    }

    void disableAllComponents()
    {

        foreach (var VARIABLE in PcComponents)
        {
            VARIABLE.gameObject.SetActive(false);
        }
        
    }
    

    public void enableObject(int index)
    {

        disableAllComponents();

        PcComponents[index].gameObject.SetActive(true);
        PcComponents[index].transform.position = pointerObject.transform.position;


    }
    
}
