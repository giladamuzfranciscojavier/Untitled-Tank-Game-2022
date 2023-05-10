using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    Canvas panel;

    void Awake()
    {
        panel=this.GetComponent<Canvas>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameUI");

        if(objs.Length > 1){
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
