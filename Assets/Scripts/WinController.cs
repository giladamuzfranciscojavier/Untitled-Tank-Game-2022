using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{

    [SerializeField] GameObject panel;

    void Awake()
    {
        panel.SetActive(false);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Win");

        if(objs.Length > 1){
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    } 

    void Nope(){
        panel.SetActive(false);
        //Camera.main.GetComponent<AudioSource>().Stop();
        GameManager.UnPause();
        GameManager.GameOver();
    }

    void NGPlus()
    {
        panel.SetActive(false);
        SceneManager.LoadScene(1);
        new WaitForSeconds(2);
        GameManager.UnPause();
    }


    void Open(){
        panel.SetActive(true);
    }
}
