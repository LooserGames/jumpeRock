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
        Vector3 pos=new Vector3(transform.position.x,target.transform.position.y,target.transform.position.z);
        this.transform.LookAt(pos);
       // transform.rotation=Quaternion.LookRotation(target.position);
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition;
        if (target.GetComponent<rockControll>().isMoving)
        {
            smoothedPosition = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x,desiredPosition.y,transform.position.z), smoothSpeed);

        }
        else
        {
            smoothedPosition = Vector3.Lerp(transform.position, new Vector3(7f,desiredPosition.y,transform.position.z), smoothSpeed);

        }
        transform.position = smoothedPosition;


       
      
    }

    
}
