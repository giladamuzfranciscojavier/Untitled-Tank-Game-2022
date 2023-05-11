using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    [SerializeField] float damage;
    bool cooldown=false;


    //El mismo script que ShootController, pero sin destruir el disparo

    private void OnTriggerStay2D(Collider2D other) {         
        if((other.tag=="Player" || other.tag=="Engine" || other.tag=="LeftTrack" || other.tag=="RightTrack") && !cooldown){
            object[] response = new object[2];
            response[0]=damage;
            response[1]=other.tag;
            StartCoroutine("Cooldown");
            other.gameObject.SendMessageUpwards("Damage",response);
        }
    }

    IEnumerator Cooldown(){
        cooldown=true;
        yield return new WaitForSeconds(.01f);
        cooldown=false;
    }


    /*private void OnTriggerEnter2D(Collider2D other) {         
        if((other.tag=="Player" || other.tag=="Engine" || other.tag=="LeftTrack" || other.tag=="RightTrack")){
            object[] response = new object[2];
            response[0]=1;
            response[1]=other.tag;
            other.gameObject.SendMessageUpwards("Damage",response);
        }
    }*/
}
