using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    [SerializeField] GameObject LTank;
    [SerializeField] GameObject MTank;
    [SerializeField] GameObject HTank;


    //A diferencia de un sistema de vidas se emplea la puntuación (dinero), que además se podrá usar para mejoras

    public void BuyTank()
    {
        if(GameManager.money>=1000){Spawn(GameManager.select);
        }    
        else{
            GameManager.GameOver();
        }    
    }

    void Start(){
        BuyTank();
    }


    void Spawn(float select){
        switch (select)
        {
            case 1:
                GameManager.UpdateMoney(-1000);
                Instantiate(LTank, transform.position,transform.rotation);
                break;
            case 2:
                GameManager.UpdateMoney(-1000);
                Instantiate(MTank, transform.position, transform.rotation);
                break;
            case 3:
                GameManager.UpdateMoney(-1000);
                Instantiate(HTank, transform.position, transform.rotation);
                break;
        }
    }
}
