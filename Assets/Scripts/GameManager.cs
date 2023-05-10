using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int money {get;private set;} = 5000;                                      //Dinero inicial
    public static int cost {get;private set;} = 1000;                                       //El coste de cada tanque. En la configuración inicial equivale a 5 vidas
    public static List<int> enemySpawns {get;private set;} = new List<int>{0,50,100,200};   //El número de enemigos que aparecerán en cada nivel (el primero es el menú principal)
    public static List<int> tanks {get;private set;} = new List<int>{1,2,3};                //Índice de tanques (1=ligero, 2=medio y 3=pesado)
    public static int select {get;private set;}=1;                                          //Tanque elegido (por defecto el ligero)
    public static float armorLevel {get;private set;} = 1;
    public static float damageLevel {get;private set;} = 1;
    public static float speedLevel {get;private set;} = 1;
    public static bool isPaused {get;private set;}=false;

    public static void Pause(){
        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach(AudioSource a in allAudioSources){
            if(a.isPlaying){                
                a.Pause();
            }
        }        
        Time.timeScale=0;
        isPaused=true;
    }

    public static void UnPause(){
        AudioSource[] allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach(AudioSource a in allAudioSources){
            if(a.time>0){
                a.Play();
            }
        }
        Time.timeScale=1;
        new WaitForSeconds(10f);
        isPaused=false;
    }

    public static void UpdateMoney(int m){money+=m;}
    public static void ClearMoney(){money=5000;}

    public static void UpgradeArmor(){
        Upgrade(armorLevel);
    }

    public static void UpgradeDamage(){
        Upgrade(armorLevel);
    }

    public static void UpgradeSpeed(){
        Upgrade(armorLevel);
    }


    public static void ClearUpgrades(){
        armorLevel=speedLevel=damageLevel=1;
    }

    public static void NewGame(){
        ClearMoney();
        ClearUpgrades();
        SceneManager.LoadScene(1);
    }

    public static void SetLight(){
        select=1;
        NewGame();
    }

    public static void SetMid(){
        select=2;
        NewGame();
    }

    public static void SetHeavy(){
        select=3;
        NewGame();
    }  
    

    public static void GameOver(){
        ClearMoney();
        ClearUpgrades();
        Destroy(GameObject.FindGameObjectWithTag("GameUI"));        
        SceneManager.LoadScene(0);
        UnPause();
    }


    public static void NextLevel(){
        int next = SceneManager.GetActiveScene().buildIndex+1;
        if(SceneManager.GetActiveScene().buildIndex<enemySpawns.Count-1){
            SceneManager.LoadScene(next);
        }
        else{
            GameOver();
        }
    }















        private static float Upgrade(float up) {
        switch (up)
        {
            case 1:
                up=1.25f;
                break;
            case 1.25f:
                up=1.5f;
                break;
            case 1.5f:
                up=1.5f;
                break;
            case 1.75f:
                up=2f;
                break;
            case 2:
                up=2;
                break;
            default:
                break;
        }
        return up;
    }
}
