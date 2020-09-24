using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLayer : MonoBehaviour
{
    // Update is called once per frame

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "engel")
        {
            other.gameObject.SetActive(false);
        }
    }
}
