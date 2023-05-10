using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] GameObject death;
    [SerializeField] Slider healthBar;
    [SerializeField] Animator[] anim;
    Rigidbody2D rb;
    NavMeshAgent agent;
    float stop;

    float health;

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
        Vector2 direction = player.transform.position - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle-90,Vector3.forward);
        this.transform.rotation =  rotation;
        float distVec = Vector3.Distance(player.transform.position, this.transform.position);  
        
        if(distVec<stop*.5f)
        {   
            agent.isStopped=false;
            Vector3 distance = this.transform.position - player.transform.position;

            Vector3 repel = this.transform.position + distance;

            agent.SetDestination(repel);
            agent.stoppingDistance=0;
        }

        else if(distVec<=stop && distVec>=stop*.5f || distVec<=stop && distVec<stop){
            rb.velocity = Vector3.zero;
            agent.isStopped=true;
        }

        else{
            agent.isStopped=false;
            agent.SetDestination(player.transform.position);
            agent.stoppingDistance=stop;
        }        
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



    //Método para manejar muerte del enemigo. Cada enemigo muerto dará 100 de dinero multiplicado por el número de nivel

    void Die(){
        GameManager.UpdateMoney(100*SceneManager.GetActiveScene().buildIndex);
        Instantiate(death, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }


}
