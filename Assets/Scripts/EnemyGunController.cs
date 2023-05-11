using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGunController : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float power;
    [SerializeField] float shotLifetime;
    [SerializeField] int reloadTime;
    new AudioSource audio;
    GameObject shot;
    bool reloading;
    bool autoFire;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    //Se comprobará de que efectivamente se encuentra a una distancia lo bastante corta del jugador como para que el disparo tenga sentido (el enemigo es visible) 

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if((Mathf.Abs(this.transform.position.x-player.transform.position.x))<15 && (Mathf.Abs(this.transform.position.y-player.transform.position.y))<7){
            if(!reloading){
                Fire();
            }
        }

    }

    void Fire(){
        StartCoroutine("Reload");
        Quaternion actPos = new Quaternion(0,0,this.transform.rotation.z,this.transform.rotation.w);
        shot = Instantiate(bullet,this.transform.position,actPos);        
        audio.Play();        
        Rigidbody2D brb = shot.GetComponent<Rigidbody2D>();        
        brb.AddForce(this.transform.up*power);
        Destroy(shot,shotLifetime);
    }


//Manejadores de recarga
//Al iniciar la corrutina se el arma. La corrutina durará 1 segundo x "reloadTime". Al terminar se restablecerá la funcionalidad del arma

IEnumerator Reload(){
    
    reloading=true;
    while(reloading){
        for (int i = 0; i < reloadTime; i++)
        {            
            yield return new WaitForSeconds(1);
        }
        reloading=false;        
    }
}

}
