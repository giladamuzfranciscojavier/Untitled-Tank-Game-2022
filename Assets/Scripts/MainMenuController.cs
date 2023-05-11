using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] GameObject mMenu;
    [SerializeField] GameObject tankSelect;
    [SerializeField] GameObject instructions;
    [SerializeField] GameObject instructions2;
    [SerializeField] GameObject controls;


    void Awake(){
        UI.SetActive(false);
    }

    void OpenSelect(){
        tankSelect.SetActive(true);
        mMenu.SetActive(false);
    }

    void OpenInstructions(){
        instructions.SetActive(true);
        instructions2.SetActive(false);
        mMenu.SetActive(false);
    }

    void OpenInstructions2(){
        instructions.SetActive(false);
        instructions2.SetActive(true);
    }

    void OpenControls(){
        controls.SetActive(true);
        mMenu.SetActive(false);
    }

    void ReturnToMain(){
        mMenu.SetActive(true);
        tankSelect.SetActive(false);
        instructions.SetActive(false);
        instructions2.SetActive(false);
        controls.SetActive(false);
    }

    void Quit(){
        Application.Quit();
    }


    void Light(){
        UI.SetActive(true);
        GameManager.SetLight();
    }

    void Mid(){
        UI.SetActive(true);
        GameManager.SetMid();
    }

    void Heavy(){
        UI.SetActive(true);
        GameManager.SetHeavy();
    }

}
