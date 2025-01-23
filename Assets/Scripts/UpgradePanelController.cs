using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelController : MonoBehaviour
{
    
    [SerializeField] Text armorText;
    [SerializeField] Button armorButton;
    [SerializeField] Text speedText;
    [SerializeField] Button speedButton;
    [SerializeField] Text damageText;
    [SerializeField] Button damageButton;

    [SerializeField] GameObject panel;

    void Awake()
    {
        panel.SetActive(false);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("UpgradeUI");

        if(objs.Length > 1){
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        Reload();
    }


    void Reload(){
        Switch(GameManager.armorLevel, armorText, armorButton);
        Switch(GameManager.speedLevel, speedText, speedButton);
        Switch(GameManager.damageLevel, damageText, damageButton);

    }


    void Switch(float level, Text txt, Button btn){
        switch (level)
        {
            case 1:
                if(GameManager.money>=1000){
                    txt.text = "Upgrade to Level 2";
                    btn.interactable=true;
                }
                else{
                    txt.text = "Not enough money";
                    btn.interactable=false;
                }
                break;
            
            case 1.25f:
                if(GameManager.money>=2000){
                    txt.text = "Upgrade to Level 3";
                    btn.interactable=true;}
                else{
                    txt.text = "Not enough money";
                    btn.interactable=false;
                }
                break;

            case 1.5f:
                if(GameManager.money>=3000){
                    txt.text = "Upgrade to Level 4";
                    btn.interactable=true;}
                else{
                    txt.text = "Not enough money";
                    btn.interactable=false;
                }
                break;
            
            case 1.75f:
                if(GameManager.money>=4000){
                    txt.text = "Upgrade to Level 5";
                    btn.interactable=true;}
                else{
                    txt.text = "Not enough money";
                    btn.interactable=false;
                }
                break;

            case 2f:
                txt.text = "MAX LEVEL";
                btn.interactable=false;
                break;

            default:
                txt.text = "Something is broken...";
                btn.interactable=false;
                break;
        }
    }




    void UpArmor(){
        GameManager.UpgradeArmor();
        Reload();
    }

    void UpSpeed(){
        GameManager.UpgradeSpeed();
        Reload();
    }

    void UpDamage(){
        GameManager.UpgradeDamage();
        Reload();
    }


    void Continue(){
        //Camera.main.GetComponent<AudioSource>().Stop();
        panel.SetActive(false);
        GameManager.NextLevel();
    }

    void Open(){
        Reload();
        panel.SetActive(true);
    }

}
