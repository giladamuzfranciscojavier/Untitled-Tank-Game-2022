using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGunController : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float power;
    [SerializeField] float shotLifetime;
    [SerializeField] int reloadTime;
    [SerializeField] GameObject aim;
    new AudioSource audio;
    Collider2D col;
    GameObject shot;
    bool reloading;
    bool autoFire;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }



    private void OnTriggerStay2D(Collider2D other) 
    {
        if(!reloading && other.gameObject.tag=="Player" && !GameManager.isPaused){                
            Fire();
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
