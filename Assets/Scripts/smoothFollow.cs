using UnityEngine;
using UnityEngine.Serialization;


/*
    *** Allows Camera to Smoothly Follow the Pointer Object,
        Which is Always at the Position of the Currently selected Object *** 

*/
public class smoothFollow : MonoBehaviour
{
   public Transform Pointertarget;
   public bool isCustomOffset;
   public Vector3 offset;

    public float smoothSpeed = 0.1f;
    
    public float zoom;
    private void Start()
    {
        // You can also specify your own offset from inspector
        // by making isCustomOffset bool to true
        if (!isCustomOffset)
        {
            offset = transform.position - Pointertarget.position;
        }
    }

    private void LateUpdate()
    {
        SmoothFollow();

        transform.LookAt(Pointertarget.transform);
    }

    public void SmoothFollow()
    {
        Vector3 targetPos = Pointertarget.position + new Vector3(offset.x+zoom,offset.y,offset.z);
        Vector3 smoothFollow = Vector3.Lerp(transform.position,
            targetPos, smoothSpeed);

        transform.position = smoothFollow;
        transform.LookAt(Pointertarget);
    }
    
    
    
    
}