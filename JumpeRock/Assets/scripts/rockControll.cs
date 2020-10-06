using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class rockControll : MonoBehaviour
{

    [SerializeField] private float rockSpeed;
    [SerializeField] private List<GameObject> engeller;
    [SerializeField] private Animator cam;
    
    
    [Header("ParticleSystem")]
    [SerializeField] private ParticleSystem rayfire;
    [SerializeField] private ParticleSystem bombFire;
    [SerializeField] private ParticleSystem meteorTile;
    [SerializeField] private ParticleSystem windTile;
    [SerializeField] private ParticleSystem snowParticle;

    [Header("audioSource")] [SerializeField]
    private AudioSource effectSound;
    [SerializeField] private AudioSource ruzgarSesi;

    [Header("Audio")] [SerializeField] private AudioClip coinSound;

    [SerializeField] private AudioClip sarkitSes;
    [SerializeField] private AudioClip sarkitSes2;
    [SerializeField] private AudioClip tahtaSes;
    [SerializeField] private AudioClip bombSound;

    [Header("PhysicMaterial")] [SerializeField]
    private PhysicMaterial wall;
    

    
    
    [Header("Panel")]
    [Header("UI")]
    [SerializeField]
    private GameObject winPanel;

    [Header("Text")] 
    [SerializeField] private Text coinText;
    [SerializeField] private Text scorText;
    [SerializeField] private Text scorXText;

    [Header("Sprite")] [SerializeField] private List<GameObject> coinImage;


    private Rigidbody rb;
    private string engelTag;
    private bool isbomb=false;
    private int coinNumber = 0;
    private int sayi = 6;
    private int carpma=3;
    private bool isCollide=false;
    private Vector3 lastPos;
    private bool carpti;
    private bool isMoving = true;
    private bool sarkitSesi;
    private bool rockVelocityY=true;
    private bool isEnd = false;
    private int coinPuan;
    private bool rockRot=true;
    
	

	
  
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(EngelList());
        cam.gameObject.GetComponent<Animator>();
        StartCoroutine(CamRot());
        StartCoroutine(RockPos());
        
        
    }

    
    

    // Update is called once per frame
    void Update()
    {
        if (rockVelocityY)
        {
            rb.velocity=new Vector3(rb.velocity.x,-360,rb.velocity.z);
        }
        

        if (rockRot)
        {
            RockMove();
            RockMoveSpeed();
            for (int i = 5; i < engeller.Count; i--)
            {
                if (engeller[i].activeSelf)
                {
                    if (carpti)
                    {
                        engeller[i].transform.rotation*=Quaternion.Euler(0,0,-1*Time.deltaTime*500f);
                    }
                    else
                    {
                        engeller[i].transform.rotation*=Quaternion.Euler(0,0,1*Time.deltaTime*500f);
                    }

               
                }
            }
        }

      
      
       RaycastHit hit;
       if (rb.SweepTest(transform.position, out hit, 3))
       {
           rockSpeed = 100;
       }
       else
       {
           rockSpeed = 1000;
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
       

        if (other.gameObject.tag=="final")
        {
            isEnd = true;
            StartCoroutine(Final());
        }
        
        if (transform.position.x < 0)
        {
            carpti = true;
            
        }
        else
        {
            carpti = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
       

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

        if (other.gameObject.tag == "altinAlani")
        {
            isEnd = false;
            IEnumerator camrot = CamRot();
            StopCoroutine(camrot);
            cam.gameObject.GetComponent<cameraTakip>().smoothSpeed = 0.5f;
            
        }

        if (other.gameObject.tag == "madenAlani")
        {
            cam.GetComponent<cameraTakip>().smoothSpeed = 0.04f;
            windTile.gameObject.SetActive(false);
            meteorTile.gameObject.SetActive(true);
            snowParticle.gameObject.SetActive(false);
            cam.transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y,-218.83f);
            rockSpeed = 0;
            isMoving = false;
            ruzgarSesi.Stop();
            rockRot = false;
        }
        
        
        
        if (other.gameObject.tag == "bomb")
        {
            other.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
            effectSound.clip = bombSound;
            effectSound.PlayOneShot(effectSound.clip);
            bombFire.Play();
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(rockStop(rb,other.gameObject));
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
            StartCoroutine(SkorFunc());
            coinNumber++;
            coinPuan += 100;
        }
        
        
        if (other.gameObject.tag == "engel")
        {
            StartCoroutine(ObjectDesiable(other.gameObject));

            sarkitSesi = !sarkitSesi;
            if (sarkitSesi)
            {
                effectSound.clip = sarkitSes;
            }
            else
            {
                effectSound.clip = sarkitSes2;
            }
            
           effectSound.PlayOneShot(effectSound.clip);
             
            for (int i = sayi; i <= engeller.Count; i--)
            {

                if (carpma > 0)
                {
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
                        StartCoroutine(ObjectDesiableRock(transform.gameObject, engeller[i - 1].gameObject));
                    }
                    carpma = 3;
                    
                    i = sayi;
                    
                }

                break;
            }
        }

        if (other.gameObject.tag == "kusYuvasi")
        {
            other.GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(kusyuvasiParticle(other.gameObject));
        }
       

    }


    IEnumerator kusyuvasiParticle(GameObject other)
    {
        yield return new WaitForSeconds(0);
        other.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.5f);
        other.gameObject.SetActive(false);
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
        yield return new WaitForSeconds(1f);
        
        rock.SetActive(false);
    }

    IEnumerator ObjectDesiable(GameObject other)
    {
        yield return new WaitForEndOfFrame();
        other.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
       /* yield return new WaitForSeconds(0.2f);
        other.transform.GetChild(1).gameObject.SetActive(false);*/
    }
    
    IEnumerator ObjectDesiableRock(GameObject other, GameObject color)
    {
        yield return new WaitForSeconds(0);
        other.transform.GetChild(9).gameObject.GetComponent<ParticleSystem>().Play();
        /*other.transform.GetChild(9).gameObject.GetComponent<ParticleSystemRenderer>().material.color =
            color.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color;*/
      
        other.transform.GetChild(9).gameObject.GetComponent<ParticleSystemRenderer>().material.SetColor("_Color",other.transform.GetChild(9).gameObject.GetComponent<ParticleSystemRenderer>().material.GetColor("_Color"));
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

    IEnumerator CamRot()
    {
        yield return new WaitForSeconds(0.1f);
        if(cam.transform.rotation.y==0 ||cam.transform.rotation.x==0 || rb.IsSleeping() || !cam.transform.hasChanged)
        {
            if (isEnd)
            {
                winPanel.SetActive(true); 
            }

        }

        yield return CamRot();
    }

    IEnumerator Final()
    {
        yield return new WaitForSeconds(1.0f);
        if (!winPanel.activeSelf)
        {
            winPanel.SetActive(true);
        }
    }

    IEnumerator rockStop(Rigidbody rock,GameObject other)
    {
        yield return new WaitForSeconds(0.1f);
        isMoving = false;
        rockVelocityY = false;
        isEnd = false;
        rock.drag = 50;
        
        StopCoroutine(CamRot());
        yield return new WaitForSeconds(0.10f);
        rock.drag = 0.0001f;
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-130,rb.velocity.z);
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-160,rb.velocity.z);
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-200,rb.velocity.z); 
        isMoving = true;
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-250,rb.velocity.z);
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-300,rb.velocity.z);
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-360,rb.velocity.z);
        rockVelocityY = true;
        isEnd = true;
        yield return new WaitForSeconds(1f);
        other.GetComponent<MeshCollider>().material = wall;
        StartCoroutine(CamRot());
    }
    
    IEnumerator RockPos()
    {
        yield return new WaitForSeconds(0.1f);
        if (!snowParticle.gameObject.activeSelf)
        {
            rockSpeed = 0;  
            transform.position=new Vector3(7f,transform.position.y,0);
            yield return new WaitForSeconds(1);
            transform.position=new Vector3(7f,transform.position.y,0);
            yield return new WaitForSeconds(1);
            transform.position=new Vector3(7f,transform.position.y,0);
        }
        else
        {
            yield return RockPos();
            
        }
    }

    IEnumerator SkorFunc()
    {
        yield return new WaitForEndOfFrame();
        coinImage[coinNumber].gameObject.SetActive(true);
        
        yield return new WaitForSeconds(0.35f);
        coinImage[coinNumber].gameObject.SetActive(false);
        coinText.text = coinPuan.ToString();

    }


   
}
