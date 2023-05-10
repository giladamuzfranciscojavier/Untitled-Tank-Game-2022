using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
        
    Canvas panel;

    void Awake()
    {
        panel=this.GetComponent<Canvas>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Pause");

        if(objs.Length > 1){
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    //Se cambiará el estado del menú de pausa (abierto/cerrado) al pulsar la tecla "escape"
    //No se abrirá en el menú principal

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            int sceneID = SceneManager.GetActiveScene().buildIndex;
            if(sceneID!=0){
            ChangeState();
            }
        }
    }



    void ChangeState(){
        if(!GameManager.isPaused){
            GameManager.Pause();
            panel.enabled=true;
        }
        else{
            GameManager.UnPause();
            panel.enabled=false;
        }
    }

    void MainMenu(){        
        GameManager.GameOver();
        Time.timeScale=1;
        panel.enabled=false;
    }
}
