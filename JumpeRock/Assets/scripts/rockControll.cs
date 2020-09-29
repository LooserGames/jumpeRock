using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class rockControll : MonoBehaviour
{

    [SerializeField] private float rockSpeed;
    [SerializeField] private float rockUpSpeed;
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
       RockMoveSpeed();

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
           
           isCollide = false;
       }
       else
       {
           //cam.SetBool("isCollide",false); 
         
       }


    }
    
    //Camera shake
    
   
    
    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>


    void RockMove()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 camX = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x,0,0)/2/2);
            
            
            
            if (camX.x < 0.13f)
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
    
    void RockMoveSpeed()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 camY = Camera.main.ScreenToViewportPoint(new Vector3(0,Input.mousePosition.y,0));
                
                print(camY.y);
                
                if (camY.y < 0.13f)
                {
                    //rb.MovePosition(transform.position+Vector3.left*Time.deltaTime*rockSpeed);
                    cam.GetComponent<cameraTakip>().smoothSpeed = 0.125f;
                    rb.drag = 0;
                }
                else
                {
                    //rb.MovePosition(transform.position+Vector3.right*Time.deltaTime*rockSpeed);
                    cam.GetComponent<cameraTakip>().smoothSpeed = 0.5f;
                    rb.drag = 0.5f;
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
            StartCoroutine(rayFire(rayfire.gameObject));

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
    
    IEnumerator rayFire(GameObject particle)
    {
        yield return new WaitForSeconds(0.3f);
        particle.SetActive(false);
    }

   
}
