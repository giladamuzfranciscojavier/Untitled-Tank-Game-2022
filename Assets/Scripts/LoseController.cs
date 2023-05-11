using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseController : MonoBehaviour
{

    [SerializeField] GameObject panel;

    void Awake()
    {
        panel.SetActive(false);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Lose");

        if(objs.Length > 1){
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    } 

    void Nope(){
        panel.SetActive(false);
        Time.timeScale=1;
        GameManager.GameOver();
    }

    void Open(){
        panel.SetActive(true);
    }
}
