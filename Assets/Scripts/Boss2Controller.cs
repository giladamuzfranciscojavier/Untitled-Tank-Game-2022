using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss2Controller : MonoBehaviour
{
    [SerializeField] GameObject death;
    [SerializeField] float maxHealth;
    [SerializeField] Slider healthBar;
    [SerializeField] Animator[] anim;
    [SerializeField] float rotSpeed;
    Rigidbody2D rb;
    NavMeshAgent agent;
    float stop;
    bool colCool;
    float health;
    bool aggro;

    //Se desactiva la rotación para evitar que se rompa el sprite

    void Start()
    {   
        health=maxHealth;
        agent=GetComponent<NavMeshAgent>();
        rb=GetComponent<Rigidbody2D>();
        healthBar.value = health/maxHealth;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        stop = agent.stoppingDistance;        
    }

    //Rotación del jefe

    private void FixedUpdate() {
        if(aggro){rb.AddTorque(99999*rotSpeed);}
    }

    void Update()
    {      
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(!agent.isStopped){            
            foreach(Animator a in anim){
                a.SetBool("Moving",true);                
            }            
        }
        else{
            foreach(Animator a in anim){
                a.SetBool("Moving",false);
            }    
        }

        healthBar.value = health/maxHealth;                   

        //El jefe perseguirá al jugador si este se encuentra por debajo de una distancia determinada, y dejará de perseguirlo si este se aleja de otra distancia determinada

        float distVec = Vector3.Distance(player.transform.position, this.transform.position);

        if(distVec<15){
            aggro=true;
        }

        if(distVec>40){
            aggro=false;
        }

        if((distVec<=stop && distVec<=stop) || !aggro){
            rb.velocity = Vector3.zero;
            agent.isStopped=true;
        }

        else if(aggro){
            agent.isStopped=false;
            agent.SetDestination(player.transform.position);
            agent.stoppingDistance=stop;
        }        
    }

    //De forma similar a los tanques pesados se aplica daño al tanque del jugador en caso de colisión, pero en menor medida debido a la gran masa del jefe

    private void OnCollisionStay2D(Collision2D other) {
        Rigidbody2D rbo = other.gameObject.GetComponent<Rigidbody2D>();
            if((other.gameObject.tag=="Player" || other.gameObject.tag=="Engine" || other.gameObject.tag=="LeftTrack" || other.gameObject.tag=="RightTrack") && !colCool && rb.mass>rbo.mass){
                StartCoroutine("ColCool");
                object[] response = new object[2];
                response[0] = (rb.mass-rbo.mass)*.5f;
                response[1] = other.gameObject.tag;
                other.gameObject.SendMessage("Damage",response);                
            }
    }
    IEnumerator ColCool(){
        colCool=true;
        yield return new WaitForSeconds(.2f);
        colCool=false;
    }



    //Método para manejar el daño al enemigo

    void Damage(float damage){        
        if(health-damage>0){
            health-=damage;
        }
        else{
            Die();
        }
    }



    //Método para manejar muerte del jefe. Dará 5000 de dinero.

    void Die(){
        GameManager.UpdateMoney(5000);
        Instantiate(death, this.transform.position, this.transform.rotation);
        GameManager.LevelWin();
        Destroy(this.gameObject);
    }


}
