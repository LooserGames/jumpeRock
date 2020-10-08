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
    [SerializeField] private Animator upPanel;
    
    
    [Header("ParticleSystem")]
    [SerializeField] private ParticleSystem rayfire;
    [SerializeField] private ParticleSystem bombFire;
    [SerializeField] private ParticleSystem meteorTile;
    [SerializeField] private ParticleSystem windTile;
    [SerializeField] private ParticleSystem snowParticle;
    [SerializeField] private ParticleSystem duman;

    [Header("audioSource")] [SerializeField]
    private AudioSource effectSound;
    [SerializeField] private AudioSource ruzgarSesi;
    [SerializeField] private AudioSource sceneMusic;

    [Header("Audio")] [SerializeField] private AudioClip coinSound;

    [SerializeField] private AudioClip sarkitSes;
    [SerializeField] private AudioClip sarkitSes2;
    [SerializeField] private AudioClip tahtaSes;
    [SerializeField] private AudioClip bombSound;
    [SerializeField] private AudioClip rockBreak;

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
    public bool isMoving = true;
    private bool sarkitSesi;
    private bool rockVelocityY=true;
    private bool isEnd = false;
    private int coinPuan=0;
    private bool rockRot=true;
    private int skorxPoint = 0;
    private bool altinMadeni = false;
    IEnumerator cameraRot;
    public bool isMoving2 = true;
    private bool isWall=false;
    


    // Start is called before the first frame update
    void Start()
    {
        cameraRot = CamRot();
        rb = GetComponent<Rigidbody>();
        StartCoroutine(EngelList());
        cam.gameObject.GetComponent<Animator>();
        
        //StartCoroutine(RockPos());
        StartCoroutine(other());
        StartCoroutine(LastPos());
       // StartCoroutine(meteorFalse());


    }


    private float zaman;
    // Update is called once per frame
    void Update()
    {
         zaman += Time.deltaTime;
         print(carpma);
         if (!altinMadeni)
         {
             AltinMadeniDisinda();
         }
         else
         {
             AltinMadeniIcinde();
         }
        
        if (rockVelocityY)
        {
            rb.velocity=new Vector3(rb.velocity.x,-360,rb.velocity.z);
        }
        
        if (isMoving && isMoving2)
        {
            RockMove();
            RockMoveSpeed();
        }
        if(!isMoving && !isMoving2)
        {
            if (!cam.transform.hasChanged)
            {
                transform.position = transform.position;
            }
            else
            {
                transform.position=Vector3.Slerp(transform.position, new Vector3(7f, transform.position.y, 0), Time.deltaTime*2f);
            }
        }

       
        
        if (rockRot)
        {
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
    }


    void AltinMadeniDisinda()
    {
        if (rb.velocity.magnitude > 360f && zaman>=4f)
        {
            meteorTile.gameObject.SetActive(true);
            meteorTile.Play();
            
            if(zaman>=5f)
            {
                windTile.gameObject.SetActive(true);
                windTile.Play();
            }

            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
           
        }
        
        
        else if(rb.velocity.magnitude < 360f)
        {
            windTile.gameObject.SetActive(false);
            meteorTile.gameObject.SetActive(false);
            
        }
        if (GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().Stop();
        }
    }

    void AltinMadeniIcinde()
    {
          print(rb.velocity.magnitude);
        if (!rockRot && !isMoving && rb.velocity.magnitude>0 )
        {
            windTile.gameObject.SetActive(true);
            meteorTile.gameObject.SetActive(true);
            windTile.Play();
            meteorTile.Play();
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }
        
        if(rb.velocity.magnitude<5f || rb.velocity.magnitude<10f || rb.velocity.magnitude<20f)
        {
            if (altinMadeni)
            {
                altinMadeni = false;
                isEnd = true;
            }

           
            
            windTile.gameObject.SetActive(false);
            meteorTile.gameObject.SetActive(false);
            duman.gameObject.SetActive(true);
            duman.Play();
            windTile.Stop();
            meteorTile.Stop();
            
            if (GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Stop();
            }
        }

    }

  

    void RockMove()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 camX = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x,0,0)/2/2);
            
            
            
            if (camX.x < 0.13f)
            {
                rb.velocity+=Vector3.left*Time.deltaTime*rockSpeed;
            }
            else
            { 
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
                    cam.GetComponent<cameraTakip>().smoothSpeed = 0.125f;
                }
                else
                {
                    cam.GetComponent<cameraTakip>().smoothSpeed = 0.5f;
                }
            }
        }

    
    private void OnCollisionEnter(Collision other)
    {
        isCollide = true;
        zaman = 2f;


        if (other.gameObject.tag == "engel" )
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
                    other.gameObject.GetComponent<Collider>().enabled = false;
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

                isWall = false;
                break;
            }

        }




        if (other.gameObject.tag=="final")
        {
            isEnd = true;
            StopCoroutine(cameraRot);
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

        if (isbomb)
        {
            if (other.gameObject.tag == "wall")
            {
                carpma = 0;
                isWall = true;
                StartCoroutine(isKinematic(rb,other));
                isbomb = false;
                
                
                
                if(carpma==0)
                {
                    StartCoroutine(rockDisintegration(engeller[sayi - 1].gameObject, "isCollide", true));

                    if (sayi > 2)
                    {
                        sayi--;
                        StartCoroutine(ObjectDesiableRock(transform.gameObject, engeller[sayi - 1].gameObject));
                    }

                    carpma = 3;

            

                }
                
            }
        }
        
        
        
    }

    /// <summary>
    /// Deneme Alanı
    /// </summary>
    /// <param name="other">
    ///bu alanda kodlar denenecek
    /// </param>

   /* void reverseRockLayer()
    {
        for (int i = 0; i < engeller.Count; i++)
        {
            if (!engeller[i].activeSelf)
            {
                engeller[i].SetActive(true);
                engeller[i].transform.GetChild(0).gameObject.SetActive(true);
                engeller[i].GetComponent<Collider>().enabled = true;
                sayi++;
                carpma = 3;
            }
           
        }
    }*/
   
    ////////////////////////////////////////


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "tahta")
        {
            other.gameObject.SetActive(false);
            rayfire.Play();
            effectSound.clip = tahtaSes;
            effectSound.PlayOneShot(effectSound.clip);

        }
        
        if (other.gameObject.tag == "skorx")
        {
            other.gameObject.SetActive(false);
            rayfire.Play();
            effectSound.clip = tahtaSes;
            effectSound.PlayOneShot(effectSound.clip);
            skorxPoint+=2;
            coinPuan += 10;
            coinText.text = coinPuan.ToString();


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
            altinMadeni = true;
            cam.GetComponent<cameraTakip>().smoothSpeed = 0.04f;
           // windTile.gameObject.SetActive(false);
            //meteorTile.gameObject.SetActive(true);
            //transform.GetChild(10).gameObject.SetActive(true);
           // transform.GetChild(10).GetComponent<ParticleSystem>().Play();
            
            snowParticle.gameObject.SetActive(false);
            cam.transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y,-400f);
            rockSpeed = 0;
            isMoving = false;
            isMoving2 = false;
            ruzgarSesi.Stop();
            rockRot = false;
            
            StartCoroutine(cameraRot);
        }
        
        
        
        if (other.gameObject.tag == "bomb")
        {
            other.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
            effectSound.clip = bombSound;
            effectSound.PlayOneShot(effectSound.clip);
            bombFire.Play();
            other.transform.GetChild(5).GetComponent<ParticleSystem>().Play();
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(rockStop(rb,other.gameObject));
            StartCoroutine(ObjectSetactive(other.gameObject));
            
            if (other.transform.position.x < 0)
            {
                
                rb.velocity=Vector3.right*Time.deltaTime*rockSpeed*50;
                isbomb = true;
            }
            else
            {
               
                rb.velocity=Vector3.left*Time.deltaTime*rockSpeed*50;
                isbomb = true;
                

            }
        }

        if (other.gameObject.tag == "coin")
        {
            coinImage[coinNumber].gameObject.SetActive(true);
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

                isWall = false;
                break;
            }
        }

        if (other.gameObject.tag == "kusYuvasi")
        {
            other.GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetChild(2).GetComponent<AudioSource>().Play();
            StartCoroutine(musicPause());
            StartCoroutine(kusyuvasiParticle(other.gameObject));
            //reverseRockLayer();
        }
       

    }


    IEnumerator kusyuvasiParticle(GameObject other)
    {
        yield return new WaitForSeconds(0);
        other.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        other.transform.GetChild(3).gameObject.GetComponent<ParticleSystem>().Play();
        upPanel.SetBool("kusYuvasi",true);
        int sonuc;
        
            sonuc = coinPuan * 0;
            coinPuan = sonuc;
            coinText.text = sonuc.ToString();
        

        
        
        yield return new WaitForSeconds(0.5f);
        other.gameObject.SetActive(false);
        upPanel.SetBool("kusYuvasi",false);
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
        yield return new WaitForSeconds(0.0f);
        //rock.GetComponent<Animator>().SetBool(value,boolean);
        rock.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
        rock.transform.GetChild(0).gameObject.SetActive(false);
        rock.GetComponent<Collider>().enabled = false;
        effectSound.clip = rockBreak;
        effectSound.PlayOneShot(effectSound.clip);
        
        coinPuan += 100;
        coinText.text = coinPuan.ToString();
        yield return new WaitForSeconds(0.5f);
        rock.SetActive(false);
    }

    IEnumerator ObjectDesiable(GameObject other)
    {
        yield return new WaitForEndOfFrame();
        other.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
    }
    
    IEnumerator ObjectDesiableRock(GameObject other, GameObject color)
    {
        yield return new WaitForSeconds(0);
        other.transform.GetChild(9).gameObject.GetComponent<ParticleSystem>().Play();
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
        yield return new WaitForSeconds(1f);
        // ||cam.transform.rotation.x==0
        //rb.IsSleeping() || !cam.transform.hasChanged||cam.transform.rotation.x==0
        //rb.IsSleeping() || !cam.transform.hasChanged||cam.transform.rotation.x==0 ||
        if (isEnd)
        {
                StartCoroutine(skorFinal());
                winPanel.SetActive(true); 
        }

        

        if (transform.hasChanged)
        {
            transform.hasChanged = false;
        }

        yield return CamRot();
    }

    IEnumerator Final()
    {
        yield return new WaitForSeconds(2f);
        if (!winPanel.activeSelf)
        {
            StartCoroutine(skorFinal());
            winPanel.SetActive(true);
        }
    }

    IEnumerator rockStop(Rigidbody rock,GameObject other)
    {
        yield return new WaitForSeconds(0.25f);
        rockVelocityY = false;
        isMoving = false;
        rock.drag = 5000000;
        
        yield return new WaitForSeconds(0.1f);
        rock.drag = 0.0001f;
        
        yield return new WaitForSeconds(0.3f);
        rb.velocity=new Vector3(rb.velocity.x,-130,rb.velocity.z);
        yield return new WaitForSeconds(0.6f);
        rb.velocity=new Vector3(rb.velocity.x,-160,rb.velocity.z);
        isMoving = true;
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-200,rb.velocity.z);
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-250,rb.velocity.z);
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-300,rb.velocity.z);
        yield return new WaitForSeconds(1f);
        rb.velocity=new Vector3(rb.velocity.x,-360,rb.velocity.z);
        rockVelocityY = true;
       

    }
    
    IEnumerator RockPos()
    {
        yield return new WaitForSeconds(0.1f);
        if (!snowParticle.gameObject.activeSelf)
        {
            rockSpeed = 0;
            transform.position =
                Vector3.Slerp(transform.position, new Vector3(7f, transform.position.y, 0), Time.deltaTime*15f);
            yield return new WaitForSeconds(1);
            transform.position=Vector3.Slerp(transform.position, new Vector3(7f, transform.position.y, 0), Time.deltaTime*15f);
            yield return new WaitForSeconds(1);
            transform.position=Vector3.Slerp(transform.position, new Vector3(7f, transform.position.y, 0), Time.deltaTime*15f);
        }
        else
        {
            yield return RockPos();
            
        }
    }

    IEnumerator SkorFunc()
    {
        yield return new WaitForSeconds(0.35f);
        coinImage[coinNumber].gameObject.SetActive(false);
        coinText.text = coinPuan.ToString();

    }

    IEnumerator skorFinal()
    {
        yield return new WaitForEndOfFrame();
        int sonuc = coinPuan * skorxPoint;
        scorXText.text = skorxPoint.ToString()+"X";
        scorText.text = sonuc.ToString();
    }

    IEnumerator musicPause()
    {
        yield return new WaitForSeconds(0);
        sceneMusic.Pause();
        yield return new WaitForSeconds(0.7f);
        sceneMusic.UnPause();
    }

    IEnumerator LastPos()
    {
        yield return new WaitForSeconds(1);
        lastPos = transform.position;
        yield return LastPos();
    }
    IEnumerator other()
    {
        yield return new WaitForSeconds(6);
        
    }

    IEnumerator isKinematic(Rigidbody rb,Collision other)
    {
        yield return new WaitForEndOfFrame();
        rb.isKinematic = true;
        isMoving = false;
        yield return new WaitForSeconds(0.10f);
        rb.isKinematic = false;
        isMoving = true;
    }

    
   
}
