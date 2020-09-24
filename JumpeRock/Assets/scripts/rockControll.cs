using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class rockControll : MonoBehaviour
{

    [SerializeField] private float rockSpeed;
    [SerializeField] private List<GameObject> engeller;
    [SerializeField] private List<Texture> catlamaTexture;


    private Rigidbody rb;
    private string engelTag;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(EngelList());
    }

    // Update is called once per frame
    void Update()
    {
       RockMove();
       
       
    }

    void RockMove()
    {
        if (Input.GetMouseButton(0))
        {
            float x = Input.mousePosition.x/2;
            if (x < 320)
            {
                rb.MovePosition(transform.position+Vector3.left*Time.deltaTime*rockSpeed);
            }
            else
            {
                rb.MovePosition(transform.position+Vector3.right*Time.deltaTime*rockSpeed);
            }
        }
    }
    
    
    
    private int sayi = 6;
    private int carpma=3;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "engel")
        {
            for (int i = sayi; i <= engeller.Count; i--)
            {

                if (carpma >= 0)
                {
                    if (carpma == 3)
                    {
                        engeller[i - 1].GetComponent<MeshRenderer>().material.mainTexture = catlamaTexture[0];
                        engeller[i - 1].GetComponent<MeshRenderer>().material.mainTextureScale=new Vector2(1f,1f);
                        engeller[i - 1].GetComponent<MeshRenderer>().material.mainTextureOffset =
                            new Vector2(0.25f,0.4f);

                    } 
                    else if (carpma == 2)
                    {
                        engeller[i - 1].GetComponent<MeshRenderer>().material.mainTexture = catlamaTexture[1];
                        engeller[i - 1].GetComponent<MeshRenderer>().material.mainTextureScale=new Vector2(1f,1f);
                        engeller[i - 1].GetComponent<MeshRenderer>().material.mainTextureOffset =
                            new Vector2(0.25f,0.3f);

                    }
                    
                    else if (carpma == 1)
                    {
                        engeller[i - 1].GetComponent<MeshRenderer>().material.mainTexture = catlamaTexture[2];
                        engeller[i - 1].GetComponent<MeshRenderer>().material.mainTextureScale=new Vector2(1f,1f);
                        engeller[i - 1].GetComponent<MeshRenderer>().material.mainTextureOffset =
                            new Vector2(0.3f,0f);

                    }
                    
                   else if (carpma==0)
                    {
                        engeller[i-1].SetActive(false);
                        if (sayi > 2)
                        {
                            sayi--;
                        }
                        carpma = 3;
                    }
                    
                    
                    other.gameObject.SetActive(false);

                    carpma--;
                    
                    break;
                }
                else
                {
                    i = sayi;
                    break;
                }
            }
        }

        Camera.main.transform.Rotate(Vector3.Lerp(new Vector3(0,0,45f),new Vector3(0,0,0),Time.deltaTime*10000f ));
    }

    IEnumerator EngelList()
    {
        yield return new WaitForEndOfFrame();
        foreach (var a in GameObject.FindGameObjectsWithTag("rock"))
        {
            engeller.Add(a.gameObject);
        }
    }
}
