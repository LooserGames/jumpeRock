using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiScript : MonoBehaviour
{

    [Header("Ui Animations")] [SerializeField]
    private Animator great;

    [SerializeField] private Animator skorX;
    [SerializeField] private Animator scor;
    [SerializeField] private Animator cleam;

    [Header("AudioSource")] [SerializeField]
    private AudioSource greatSound;
    [SerializeField] private AudioSource skorXSound;
    [SerializeField] private AudioSource scorSound;
    [SerializeField] private AudioSource cleamSound;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UiAnimatonPlay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UiAnimatonPlay()
    {
        yield return new WaitForSeconds(0.1f);
        great.SetBool("great",true);
        greatSound.PlayOneShot(greatSound.clip);

        yield return new WaitForSeconds(0.5f);
        great.SetBool("great",false);
        skorX.SetBool("skorx",true);
        skorXSound.PlayOneShot(skorXSound.clip);
        
        yield return new WaitForSeconds(0.5f);
        skorX.SetBool("scorx",false);
        scor.SetBool("scor",true);
        scorSound.PlayOneShot(scorSound.clip);
        
        yield return new WaitForSeconds(0.5f);
        scor.SetBool("scor",false);
        cleam.SetBool("cleam",true);
        cleamSound.PlayOneShot(cleamSound.clip);

        
        yield return new WaitForSeconds(0.5f);
        cleam.SetBool("cleam",false);
    }
}
