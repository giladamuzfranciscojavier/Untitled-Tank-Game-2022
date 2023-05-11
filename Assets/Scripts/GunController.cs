using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float power;
    [SerializeField] float shotLifetime;
    [SerializeField] bool isMinigun;
    [SerializeField] int reloadTime;
    new AudioSource audio;
    GameObject shot;
    Text UIGun;
    bool reloading;
    bool autoFire;

    void Start()
    {
        //Se ajusta la fuerza de disparo con las mejoras de velocidad para evitar situaciones en las que el tanque sea más rápido que los proyectiles

        power*=GameManager.speedLevel;

        if(this.gameObject.tag=="PrimGun"){
            UIGun = GameObject.FindGameObjectWithTag("UIGun1").GetComponent<Text>();
            UIGun.text="1st Gun:\nLoaded!";
        }
        
        if(this.gameObject.tag=="SecGun"){
            UIGun = GameObject.FindGameObjectWithTag("UIGun2").GetComponent<Text>();
            if(isMinigun){
                UIGun.text="2nd Gun:\nFull Auto!";
            }
            else{
                UIGun.text="2nd Gun:\nLoaded!";
            }
        }

        audio = GetComponent<AudioSource>();
    }

    void Update()
    {        
        if(this.gameObject.tag=="PrimGun" && Input.GetButtonDown("Fire1") && !GameManager.isPaused){
            if(!reloading){
                Fire();
            }
        }

        //Si el arma es una ametralladora se disparará de forma contínua mientras se pulse el botón. En el caso contrario solo se disparará una vez por pulsación

        if(this.gameObject.tag=="SecGun" && !isMinigun && Input.GetButtonDown("Fire2") && !GameManager.isPaused){
            if(!reloading){
                Fire();
            }
        }

        if(this.gameObject.tag=="SecGun" && isMinigun && Input.GetButton("Fire2") && !GameManager.isPaused){            
            
            autoFire=true;

            if(!reloading){
                AutoFire();
            }
        }

        if(this.gameObject.tag=="SecGun" && isMinigun && Input.GetButtonUp("Fire2") && !GameManager.isPaused){     

            autoFire=false;

        }

        if(isMinigun){
            if(autoFire && !audio.isPlaying){
                audio.Play();
            }
            if(!autoFire && audio.isPlaying){
            audio.Stop();
        }
        }

    }


    void Fire(){
        StartCoroutine("Reload");
        shot = Instantiate(bullet,this.transform.position,this.transform.rotation);
        if(!GameManager.isPaused){audio.Play();}     
        Rigidbody2D brb = shot.GetComponent<Rigidbody2D>();
        brb.AddForce(this.transform.up*power);        
        Destroy(shot,shotLifetime);
    }

    void AutoFire(){
        StartCoroutine("AutoReload");
        shot = Instantiate(bullet,this.transform.position,this.transform.rotation);        
        Rigidbody2D brb = shot.GetComponent<Rigidbody2D>();
        brb.AddForce(this.transform.up*power);
        Destroy(shot,shotLifetime);
    }


//Manejadores de recarga
//Al iniciar la corrutina se inutilizará el armas. La corrutina durará 1 segundo x "reloadTime". Al terminar se restablecerá la funcionalidad del arma
//En el caso de la ametralladora ligera el propósito será mantener una cadencia de fuego aceptable (3000 disparos por minuto)

IEnumerator Reload(){
    
    if(this.gameObject.tag=="PrimGun"){
        UIGun.text="1st Gun:\nReloading...("+reloadTime+")";
    }
    else{
        UIGun.text="2nd Gun:\nReloading...("+reloadTime+")";
    }
    
    reloading=true;
    while(reloading){
        for (int i = 0; i < reloadTime; i++)
        {
            if(this.gameObject.tag=="PrimGun"){
                UIGun.text="1st Gun:\nReloading...("+(reloadTime-i)+")";
            }
            else{
                UIGun.text="2nd Gun:\nReloading...("+(reloadTime-i)+")";
            }
            yield return new WaitForSeconds(1);
        }
        if(this.gameObject.tag=="PrimGun"){
            UIGun.text="1st Gun:\nLoaded!";
        }
        else{
            UIGun.text="2nd Gun:\nLoaded!";
        }
        reloading=false;        
    }
}

IEnumerator AutoReload(){    
    reloading=true;
    while(reloading){
        yield return new WaitForSeconds(.02f);
        reloading=false;
    }
}

}
