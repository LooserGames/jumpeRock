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
    
    
    [Header("ParticleSystem")]
    [SerializeField] private ParticleSystem rayfire;
    [SerializeField] private ParticleSystem bombFire;
    [SerializeField] private ParticleSystem meteorTile;
    [SerializeField] private ParticleSystem windTile;

    [Header("audioSource")] [SerializeField]
    private AudioSource effectSound;

    [Header("Audio")] [SerializeField] private AudioClip coinSound;

    [SerializeField] private AudioClip sarkitSes;
    [SerializeField] private AudioClip tahtaSes;
    [SerializeField] private AudioClip bombSound;


    private Rigidbody rb;
    private string engelTag;
    private bool isbomb=false;
    
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
           rockSpeed = 3000;
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
        rb.velocity=new Vector3(rb.velocity.x,-360,rb.velocity.z);
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
                
                
                if (camY.y < 0.13f)
                {
                    //rb.MovePosition(transform.position+Vector3.left*Time.deltaTime*rockSpeed);
                    cam.GetComponent<cameraTakip>().smoothSpeed = 0.125f;
                   // rb.drag = 0;
                }
                else
                {
                    //rb.MovePosition(transform.position+Vector3.right*Time.deltaTime*rockSpeed);
                    cam.GetComponent<cameraTakip>().smoothSpeed = 0.5f;
                    //rb.drag = 0.5f;
                }
            }
        }

    private void OnCollisionEnter(Collision other)
    {
        isCollide = true;

        if (other.gameObject.tag == "tahta")
        {
            other.gameObject.SetActive(false);
            //rayfire.gameObject.SetActive(true);
            rayfire.Play();
            effectSound.clip = tahtaSes;
            effectSound.PlayOneShot(effectSound.clip);
            //other.gameObject.GetComponent<Animator>().SetBool("tahtaKir",true);
            // StartCoroutine(sarkitDisintegration(other.gameObject, "tahtaKir", true));
            //StartCoroutine(rayFire(rayfire.gameObject));

        }
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "madenAlani")
        {
            cam.GetComponent<cameraTakip>().smoothSpeed = 0.05f;
            windTile.gameObject.SetActive(false);
            meteorTile.gameObject.SetActive(true);
        }
        
        
        
        if (other.gameObject.tag == "bomb")
        {
            effectSound.clip = bombSound;
            effectSound.PlayOneShot(effectSound.clip);
            bombFire.Play();
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(ObjectSetactive(other.gameObject));
            if (other.transform.position.x < 0)
            {
                /*if (sayi > 2)
                {
                    sayi--;
                }

                carpma = 3;*/
                carpma = 0;
                rb.velocity=Vector3.right*Time.deltaTime*rockSpeed*300;
                isbomb = true;
                

            }
            else
            {
                carpma = 0;
                rb.velocity=Vector3.left*Time.deltaTime*rockSpeed*300;
                isbomb = true;
                

            }
        }

        if (other.gameObject.tag == "coin")
        {
            effectSound.clip = coinSound;
            effectSound.PlayOneShot(effectSound.clip);
            other.gameObject.SetActive(false);
        }
        
        
        if (other.gameObject.tag == "engel")
        {
           /* if (isbomb)
            {
                if (sayi > 2)
                {
                    sayi--;
                }
                carpma = 3;
                isbomb = false;
            }*/
           
           effectSound.clip = sarkitSes;
           effectSound.PlayOneShot(effectSound.clip);
             
            for (int i = sayi; i <= engeller.Count; i--)
            {

                if (carpma > 0)
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
                        
                    }
                    
                    
                    //other.gameObject.SetActive(false);
                    
                    StartCoroutine(sarkitDisintegration(other.gameObject, "degdi", true));
                    other.GetComponent<Collider>().enabled = false;
                    carpma--;
                   

                    break;
                }
                else
                {
                    StartCoroutine(rockDisintegration(engeller[i - 1].gameObject, "isCollide", true));
                        
                    if (sayi > 2)
                    {
                        sayi--;
                    }
                    carpma = 3;
                    
                    i = sayi;
                    
                }

                break;
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
        yield return new WaitForSeconds(3f);
        rock.SetActive(false);
    }
    
    IEnumerator sarkitDisintegration(GameObject rock,string value, bool boolean)
    {
        yield return new WaitForSeconds(0.0f);
        rock.GetComponent<Animator>().SetBool(value,boolean);
        yield return new WaitForSeconds(1f);
        rock.GetComponent<Animator>().SetBool(value,false);
        rock.SetActive(false);
    }
    
    IEnumerator rayFire(GameObject particle)
    {
        yield return new WaitForSeconds(0.3f);
        particle.SetActive(false);
    }

    IEnumerator ObjectSetactive(GameObject myObject)
    {
        yield return new WaitForSeconds(0.5f);
        myObject.SetActive(false);
    }

   
}
