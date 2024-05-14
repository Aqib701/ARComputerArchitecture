

using System;
using UnityEngine;
using System.Collections;
 

// *** ControlsRotating of Selected Objects with Respect to Mouse and Touch Inputs ***

public class SelectableObject : MonoBehaviour
{
    
    public AudioClip componentAudio;
   
    [TextArea]
    public string objectDescription;
    public float zoomAmountForFocus;
    [Space]
    [SerializeField] internal bool isSelected;
    [HideInInspector] public string objectName;
    [HideInInspector] public Vector3 defaultPosition;
    [HideInInspector] public Quaternion defaultRotation;
    
    private Transform target;
    private  float xSpeed = 200.0f;
    private  float ySpeed = 200.0f;
    private  float xDeg = 0.0f;
    private  float yDeg = 0.0f;
   // private  int yMinLimit = -80;
   // private  int yMaxLimit = 80;
   
    void Start()
    {
        var objectTransform = transform;
        
        target = objectTransform;
        Vector3 angles = objectTransform.eulerAngles;
        defaultPosition = objectTransform.localPosition;
        defaultRotation = objectTransform.localRotation;
        
        xDeg = angles.x;
        yDeg = angles.y;
        objectName = gameObject.name;
    }
    
  
    void LateUpdate()
    {
        
        
        // Don't do anything if target is not defined or is not selected
        if (!target || !isSelected)
            return;
 
        // If middle mouse selected? ORBIT
        if( Input.GetButton("Touch"))
        {
            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }
         
 
        //Clamp the vertical axis for the orbit
    //    yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
    // set camera rotation
        Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0);
        
        transform.rotation = rotation;
        

    }
 
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
    
    
    
}
 