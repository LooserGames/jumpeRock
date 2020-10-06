using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject startPanel;
    void Start()
    {
        rock.GetComponent<rockControll>().enabled = false;
        rock.GetComponent<Rigidbody>().isKinematic = true;
        startPanel.SetActive(false);
        camera.GetComponent<Animator>().enabled = true;
        camera.GetComponent<cameraTakip>().enabled = false;

        StartCoroutine(ComponentsTrueandFalse());

    }

   

    public void StartFunc()
    {
        rock.GetComponent<rockControll>().enabled = true;
        rock.GetComponent<Rigidbody>().isKinematic = false;
        camera.GetComponent<Animator>().enabled = false;
        camera.GetComponent<cameraTakip>().enabled = true;
        startPanel.SetActive(false);
        
    }

    IEnumerator ComponentsTrueandFalse()
    {
        yield return new WaitForSeconds(10f);
        if (!startPanel.activeSelf)
        {
            startPanel.SetActive(true);
        }
        
    }
}
