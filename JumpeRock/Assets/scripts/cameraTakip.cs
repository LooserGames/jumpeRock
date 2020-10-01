using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTakip : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate ()
    {
        this.transform.LookAt(target);
       // transform.rotation=Quaternion.LookRotation(target.position);
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x,desiredPosition.y,transform.position.z), smoothSpeed);
        transform.position = smoothedPosition;


       
      
    }

    
}
