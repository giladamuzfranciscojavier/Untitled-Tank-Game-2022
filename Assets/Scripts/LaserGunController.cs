using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGunController : MonoBehaviour
{
    [SerializeField] GameObject laser;
    Animator lanim;
    new AudioSource audio;
    Collider2D col;
    GameObject shot;
    bool reloading;
    bool autoFire;

    void Start()
    {
        lanim=laser.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }



    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Player" && !GameManager.isPaused){                
            lanim.SetBool("Firing",true);
            if(!audio.isPlaying){audio.Play();}
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag=="Player" && !GameManager.isPaused){                
            lanim.SetBool("Firing",false);
            audio.Stop();
        }        
    }
}

