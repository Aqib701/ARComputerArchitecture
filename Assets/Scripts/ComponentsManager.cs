using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class ComponentsManager : MonoBehaviour
{


    [Serializable]
    public class PCobjects
    {
        public panAndRotate Objects;
        public GameObject detailsCanvas;
        public void disable()
        {
            Objects.isSelected = false;
           Objects.stopComponentAudio();
            Objects.enabled = false;
            detailsCanvas.SetActive(false);

        }
        
        public void enable()
        {

            Objects.enabled = true;
            detailsCanvas.SetActive(true);

        }
        
    }

    public enum zoomAxis
    {
        X,Y,Z
        
    }

    public bool enableDefaultObjAtStart;
    public float selectionOffset=10;
    public zoomAxis selectionZoomAxis=zoomAxis.Y;
    [Space]
    public PCobjects currentSelectedObject;
    [Space] public smoothFollow CameraSmoothFollow;
    public Transform pointerObject;
    public float pointerMoveTime = 1f;
    public Vector3 pointerOffset;
    public PCobjects[] rotateObjects;


    private Vector3 defaultPointerPosition;
    
    
    void Start()
    {
        defaultPointerPosition = pointerObject.position;
        
        if (!enableDefaultObjAtStart)
        {
            resetCamera();
            return;
        }

        disablePanForEveryObj();
        rotateObjects[0].enable(); // Enable the Whole PC obj
    }
    

    public void resetCamera()
    {
        
      
        StartCoroutine(movePointer( defaultPointerPosition, pointerMoveTime,false));
                 
        
        
        if (CameraSmoothFollow.GetComponent<Camera>().orthographic)
        {

            CameraSmoothFollow.GetComponent<Camera>().orthographicSize = 20;
        }

        
                    
        rotateObjects[0].Objects.enabled = true;
        rotateObjects[0].Objects.canvasObject.SetActive(true);
        CameraSmoothFollow.zoom = rotateObjects[0].Objects.zoomAmountForFocus;
        disablePanForEveryObj();
        
    }

    void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
            RaycastHit hit;
         
            if( Physics.Raycast( ray, out hit, 10000 ) )
            {
                Debug.Log( hit.transform.gameObject.name ); // Object to Select Found with Raycast Hit
                
                if (hit.transform.gameObject.GetComponents<panAndRotate>()!= null)
                {
                    
                    if (hit.transform.gameObject.GetComponent<panAndRotate>().isSelected)
                            return;
                    else
                    {
                        disablePanForEveryObj();
                        hit.transform.gameObject.GetComponent<panAndRotate>().isSelected = true;
                    }
                        
                        
                    currentSelectedObject.Objects = hit.transform.gameObject.GetComponent<panAndRotate>();

                    //** Object Position Correction According to their Axis from line 121 - 155 **
                    
                    if (selectionZoomAxis == zoomAxis.Y)
                    {
                        currentSelectedObject.Objects.gameObject.transform.position = new Vector3(
                            currentSelectedObject.Objects.gameObject.transform.position.x,
                            currentSelectedObject.Objects.gameObject.transform.position.y + selectionOffset,
                            currentSelectedObject.Objects.gameObject.transform.position.z);
                    }
                    else 
                    if (selectionZoomAxis == zoomAxis.Z)
                    {
                        currentSelectedObject.Objects.gameObject.transform.position = new Vector3(
                            currentSelectedObject.Objects.gameObject.transform.position.x,
                            currentSelectedObject.Objects.gameObject.transform.position.y ,
                            currentSelectedObject.Objects.gameObject.transform.position.z+ selectionOffset);
                    }

                    else 
                    
                    if (selectionZoomAxis == zoomAxis.X)
                    {
                        currentSelectedObject.Objects.gameObject.transform.position = new Vector3(
                            currentSelectedObject.Objects.gameObject.transform.position.x + selectionOffset,
                            currentSelectedObject.Objects.gameObject.transform.position.y,
                            currentSelectedObject.Objects.gameObject.transform.position.z);
                    }



                    if (CameraSmoothFollow.GetComponent<Camera>().orthographic)
                    {

                        CameraSmoothFollow.GetComponent<Camera>().orthographicSize =
                          currentSelectedObject.Objects.zoomAmountForFocus;
                    }
                    
                    // Moving Pointer Towards the Selected Object
                    StartCoroutine(movePointer(currentSelectedObject.Objects.transform.position, pointerMoveTime,true));
                    currentSelectedObject.Objects.enabled = true;
                    currentSelectedObject.Objects.canvasObject.SetActive(true);
                    currentSelectedObject.Objects.playComponentAudio();
                    CameraSmoothFollow.zoom = currentSelectedObject.Objects.zoomAmountForFocus;
                    

                }
                
            }
        }
    }



    void disablePanForEveryObj()
    {

        foreach (var VARIABLE in rotateObjects)
        {
          
            VARIABLE.disable();
            VARIABLE.Objects.gameObject.transform.localPosition = VARIABLE.Objects.defaultPosition;
            VARIABLE.Objects.gameObject.transform.localRotation = VARIABLE.Objects.defaultRotation;

        }
        
    }


    IEnumerator movePointer(Vector3 destPoint, float time, bool withOffset)
    {
        float timeElapsed = 0;
        while (timeElapsed < time)
        {
            pointerObject.transform.position = Vector3.Lerp( pointerObject.transform.localPosition, destPoint, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if(withOffset)
             pointerObject.transform.position = destPoint+ pointerOffset;
        else
        {
            pointerObject.transform.position = destPoint;
        }
        
    }
}
