using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.Serialization;

public class ComponentsManager : MonoBehaviour
{
   
    
    public enum ZoomAxis
    {
        X,Y,Z
        
    }
    
    [SerializeField] float selectionOffset=10;
    public ZoomAxis selectionZoomAxis=ZoomAxis.Y;
    public LayerMask collisionLayer;
    [Space]
    public Transform pointerObject;
    public Transform selectableObjectsParent;
  
    [Space]    

    [SerializeField] float pointerMoveTime = 1f;
    [SerializeField] Vector3 pointerOffset;
    [SerializeField] SelectableObject[] selectableObjects;
    
    private Vector3 _defaultPointerPosition;
    private Camera _mainCamera;
    private int _defaultObjectIndex =0 ;
    private smoothFollow _cameraSmoothFollow; 
    private SelectableObject currentSelectedObject;
    void Start()
    {
        _mainCamera = Camera.main;
        _defaultPointerPosition = pointerObject.position;
        Debug.Log("defaultPointerPosition: "+_defaultPointerPosition);
        selectableObjects = selectableObjectsParent.GetComponentsInChildren<SelectableObject>();
        if (_mainCamera != null)  _cameraSmoothFollow = _mainCamera.GetComponent<smoothFollow>();
        selectableObjects[_defaultObjectIndex].gameObject.SetActive(true); // Enable the WholePC Obj
    }
    

    public void ResetCamera()
    {
        
        
        Debug.Log("reset Camera");
        
        StartCoroutine(MovePointer( _defaultPointerPosition, pointerMoveTime,false));
        if (_mainCamera.orthographic)
        {
            _mainCamera.orthographicSize = 20;
        }

        if (currentSelectedObject) DiselectObject(currentSelectedObject); // Make previously selected object false
        
        currentSelectedObject = selectableObjects[_defaultObjectIndex]; // Make currentSelected object default object
        
        currentSelectedObject.isSelected = true;
      
        _cameraSmoothFollow.zoom = currentSelectedObject.zoomAmountForFocus;
        UIManager.Instance.SetDetails(currentSelectedObject.objectName,currentSelectedObject.objectDescription); 
        
     

    }

    void Update()
    {
        if( Input.GetButtonDown("Touch"))
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
                RaycastHit hit;
         
                if( Physics.Raycast( ray, out hit, 200,collisionLayer ) )
                {
                    Debug.Log(hit.transform.gameObject.name ); // Object to Select Found with Raycast Hit
                    
                    var hitObj=hit.transform.gameObject.GetComponent<SelectableObject>();
                    
                    
                    if (hitObj.isSelected)  // If hit the object which already selected then do nothing
                        return;
                    
                        hitObj.isSelected = true;
                        if (currentSelectedObject) DiselectObject(currentSelectedObject); // Make previously selected object false
                        currentSelectedObject = hitObj; // Make currentSelected object what we selected just now
                        
                      SelectObject(currentSelectedObject);
                        
                    var currentSelectedObjectPosition = currentSelectedObject.transform.position;
                    
                    
                    //** Object Position Correction According to their Axis 
                    
                    if (selectionZoomAxis == ZoomAxis.Y)
                    {
                        currentSelectedObject.gameObject.transform.position = new Vector3(
                            currentSelectedObjectPosition.x,
                            currentSelectedObjectPosition.y + selectionOffset,
                            currentSelectedObjectPosition.z);
                    }
                    else 
                    if (selectionZoomAxis == ZoomAxis.Z)
                    {
                        currentSelectedObject.gameObject.transform.position = new Vector3(
                            currentSelectedObjectPosition.x,
                            currentSelectedObjectPosition.y ,
                            currentSelectedObjectPosition.z+ selectionOffset);
                    }

                    else
                    if (selectionZoomAxis == ZoomAxis.X)
                    {
                        currentSelectedObject.gameObject.transform.position = new Vector3(
                            currentSelectedObjectPosition.x + selectionOffset,
                            currentSelectedObjectPosition.y,
                            currentSelectedObjectPosition.z);
                    }
                    
                    if (_mainCamera.orthographic)
                    {

                        _mainCamera.orthographicSize =
                            currentSelectedObject.zoomAmountForFocus;
                    }
 
                }
            }
        }
    }



    void SelectObject(SelectableObject Obj)
    {
        
        Obj.isSelected = true;
        AudioManager.instance.PlayComponentAudio(Obj.componentAudio);
        _cameraSmoothFollow.zoom = Obj.zoomAmountForFocus;
        UIManager.Instance.SetDetails(Obj.objectName,currentSelectedObject.objectDescription); 
        
        StartCoroutine(MovePointer(currentSelectedObject.transform.position, pointerMoveTime,true));

        
    }
    
    void DiselectObject(SelectableObject Obj)
    {
        
        Obj.isSelected = false;
        
        var objtransform = Obj.transform;
        objtransform.localPosition = Obj.defaultPosition;
        objtransform.localRotation = Obj.defaultRotation;
        
        AudioManager.instance.StopComponentAudio();
        
        
    }
    
    IEnumerator MovePointer(Vector3 destPoint, float time, bool withOffset)
    {
        float timeElapsed = 0;


        if (withOffset)
        {
            Debug.Log("Using Offset: "+withOffset);
            destPoint += pointerOffset;
        }
        
        
        while (timeElapsed < time)
        {
            pointerObject.transform.localPosition = Vector3.Lerp( pointerObject.transform.localPosition, destPoint, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // if(withOffset)
        //      pointerObject.transform.position = destPoint+ pointerOffset;
        // else
        
        Debug.Log("defaultPointerPosition: "+destPoint);
            pointerObject.transform.localPosition = destPoint;
        
        
    }
    
     
  

}
