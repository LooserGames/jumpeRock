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
    [SerializeField] private Animator cam;
    [SerializeField] private ParticleSystem rayfire;


    private Rigidbody rb;
    private string engelTag;
    
    private int sayi = 6;
    private int carpma=3;
    private bool isCollide=false;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(EngelList());
        cam.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       RockMove();

       RaycastHit hit;
       if (rb.SweepTest(transform.position, out hit, 3))
       {
           rockSpeed = 100;
       }
       else
       {
           rockSpeed = 200;
       }

       if (isCollide)
       {
           cam.SetBool("isCollide",true);
           isCollide = false;
       }
       else
       {
           cam.SetBool("isCollide",false); 
       }


    }

    void RockMove()
    {
        if (Input.GetMouseButton(0))
        {
            float x = Input.mousePosition.x/2;
            if (x < 320)
            {
                //rb.MovePosition(transform.position+Vector3.left*Time.deltaTime*rockSpeed);
                rb.velocity+=Vector3.left*Time.deltaTime*rockSpeed;
            }
            else
            {
                //rb.MovePosition(transform.position+Vector3.right*Time.deltaTime*rockSpeed);
                rb.velocity+=Vector3.right*Time.deltaTime*rockSpeed;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        isCollide = true;

        if (other.gameObject.tag == "tahta")
        {
            other.gameObject.SetActive(false);
            rayfire.gameObject.SetActive(true);

        }
        else
        {
            rayfire.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "engel")
        {
             
            for (int i = sayi; i <= engeller.Count; i--)
            {

                if (carpma >= 0)
                {
                   /* if (carpma == 3)
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

                    }*/
                    
                    if (carpma==0)
                    {
                        //engeller[i-1].SetActive(false);
                        //engeller[i-1].GetComponent<Animator>().SetBool("isCollide",true);
                        StartCoroutine(rockDisintegration(engeller[i - 1].gameObject, "isCollide", true));
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

        
    }

    IEnumerator EngelList()
    {
        yield return new WaitForEndOfFrame();
        foreach (var a in GameObject.FindGameObjectsWithTag("rock"))
        {
            engeller.Add(a.gameObject);
        }
    }

    IEnumerator rockDisintegration(GameObject rock,string value, bool boolean)
    {
        yield return new WaitForSeconds(0.1f);
        rock.GetComponent<Animator>().SetBool(value,boolean);
        yield return new WaitForSeconds(0.7f);
        rock.SetActive(false);
    }
}
