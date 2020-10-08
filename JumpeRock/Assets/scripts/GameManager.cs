using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject poinPanel;
    void Start()
    {
        rock.GetComponent<rockControll>().enabled = false;
        rock.GetComponent<Rigidbody>().isKinematic = true;
        startPanel.SetActive(false);
        camera.GetComponent<Animator>().enabled = true;
        camera.GetComponent<cameraTakip>().enabled = false;
        poinPanel.SetActive(false);

        StartCoroutine(ComponentsTrueandFalse());

    }

   

    public void StartFunc()
    {
        StartCoroutine(isStart());
    }

    IEnumerator ComponentsTrueandFalse()
    {
        yield return new WaitForSeconds(8f);
        if (!startPanel.activeSelf)
        {
            startPanel.SetActive(true);
        }
        
    }

    IEnumerator isStart()
    {
        yield return new WaitForEndOfFrame();
        startPanel.transform.GetChild(0).GetComponent<Animator>().SetBool("info",true);
        startPanel.transform.GetChild(1).GetComponent<Animator>().SetBool("start",true);
        poinPanel.transform.GetChild(0).GetComponent<Animator>().SetBool("baslangic",true);
        yield return new WaitForSeconds(1.5f);
        startPanel.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        poinPanel.SetActive(true);
        poinPanel.transform.GetChild(0).GetComponent<Animator>().SetBool("baslangic",false);
        yield return new WaitForSeconds(0.7f);
        rock.GetComponent<rockControll>().enabled = true;
        rock.GetComponent<Rigidbody>().isKinematic = false;
        camera.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(0.54f);
        camera.GetComponent<cameraTakip>().enabled = true;
        
    }
}
