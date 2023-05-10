using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{

    [SerializeField] float damage;
    [SerializeField] GameObject explosion;


    //En caso de impactar un proyectil del jugador en un enemigo se llama a su método "Damage" enviándole el daño que inflige el proyectil

    //En caso de impactar un proyectil de un enemigo en el jugador se llama al método "Damage" del jugador enviándole un array de objetos que contiene tanto el daño del proyectil como la parte en la que este ha impactado 

    //Las minas infligen daño a todos los tanques

    private void OnTriggerEnter2D(Collider2D other) {        
        if((this.tag=="PlayerShot" || this.tag=="Mine") && other.tag=="Enemy"){
            other.SendMessage("Damage",damage);
            if(this.tag=="Mine"){GetComponent<AudioSource>().Play();Instantiate(explosion,this.transform.position,this.transform.rotation);}
            Destroy(this.gameObject);
        }
        else if((this.tag=="EnemyShot" || this.tag=="Mine") && (other.tag=="Player" || other.tag=="Engine" || other.tag=="LeftTrack" || other.tag=="RightTrack")){
            object[] response = new object[2];
            response[0]=damage;
            response[1]=other.tag;
            //Debug.Log("Infligido "+damage+" de daño en el módulo " + other.tag);
            other.gameObject.SendMessageUpwards("Damage",response);
            if(this.tag=="Mine"){GetComponent<AudioSource>().Play();Instantiate(explosion,this.transform.position,this.transform.rotation);}
            Destroy(this.gameObject);
        }

        else if (other.tag=="Wall"){
            Destroy(this.gameObject);
        }
    }
}
