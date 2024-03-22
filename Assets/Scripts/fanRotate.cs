using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanRotate : MonoBehaviour
{
    
    // Simple Script to rotate Objects On An Axis
    // Used to Rotate Fan blades

    public enum axis
    {
        X,Y,Z
    }
    public bool spinning=true;
    public axis Axis;
    public float Speed=2;

    void Update()
    {
        if (spinning)
        {
            if(Axis==axis.X)
             transform.Rotate(Vector3.right*Speed);
            
            if(Axis==axis.Y)
                transform.Rotate(new Vector3(0,1,0)*Speed);
            
            if(Axis==axis.Z)
                transform.Rotate(new Vector3(0,0,1)*Speed);
            
        }
          
    }
}
