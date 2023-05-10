using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankController : MonoBehaviour
{
    Rigidbody2D rb;
    AudioSource engine;
    [SerializeField] Animator[] anim;

    [SerializeField] GameObject death;

    Text UIHealth;
    Text UIFront;
    Text UIEngine;
    Text UIRight;
    Text UILeft;
    Text UISpeed;
    Image UIRepair;

    [SerializeField] float maxHealth;
    [SerializeField] float maxFrontArmor;
    [SerializeField] float maxEngineArmor;
    [SerializeField] float maxTrackArmor;
    [SerializeField] float acceleration;
    [SerializeField] float rotSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] public bool heavy;

    bool colCool;

    float health;
    float frontArmor;
    float engineArmor;
    float lTrackArmor;
    float rTrackArmor;

    float currentAcceleration;

    bool oneTrack=false;
    bool noTrack=false;
    bool repairing=false;
    bool abort;
    bool ded=false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        engine = GetComponent<AudioSource>();

        UIHealth = GameObject.FindGameObjectWithTag("UIHealth").GetComponent<Text>();
        UIFront = GameObject.FindGameObjectWithTag("UIFront").GetComponent<Text>();
        UIEngine = GameObject.FindGameObjectWithTag("UIEngine").GetComponent<Text>();
        UIRight = GameObject.FindGameObjectWithTag("UIRight").GetComponent<Text>();
        UILeft = GameObject.FindGameObjectWithTag("UILeft").GetComponent<Text>();
        UISpeed = GameObject.FindGameObjectWithTag("UISpeed").GetComponent<Text>();
        UIRepair = GameObject.FindGameObjectWithTag("UIRepair").GetComponent<Image>();    

        //Se aplican las mejoras. Para el tanque pesado las mejoras de armadura repercutirán también en la masa

        health=maxHealth*GameManager.armorLevel;
        frontArmor=maxFrontArmor*GameManager.armorLevel;
        engineArmor=maxEngineArmor*GameManager.armorLevel;
        lTrackArmor=rTrackArmor=maxTrackArmor*GameManager.armorLevel;
        maxSpeed*=GameManager.speedLevel;
        acceleration*=GameManager.speedLevel;

        if(heavy){rb.mass*=GameManager.armorLevel;}
    }

    void Update()
    {   

        //Si hay una oruga inutilizada se divide la aceleración real entre 2. Si las dos están inutilizadas el tanque no acelerará

        if(oneTrack){
            currentAcceleration=acceleration*.5f;
        }

        else if(noTrack){
            currentAcceleration=0;
        }

        else{
            currentAcceleration=acceleration;
        }

        //Se activará la animación de las orugas a partir de una velocidad mínima

        if(rb.velocity.magnitude>=.75){
            foreach(Animator a in anim){                
                if(!engine.isPlaying && !GameManager.isPaused){
                    engine.Play();
                }
                a.SetBool("Moving",true);                
        }            
        }
        else{
            foreach(Animator a in anim){
                a.SetBool("Moving",false);
                engine.Stop();
            }    
        }
        
        UIFront.text= "Front: "+frontArmor;
        UILeft.text= "L. Track: "+lTrackArmor;
        UIRight.text= "R. Track: "+rTrackArmor;
        UIEngine.text= "Engine: "+engineArmor;
        UIHealth.text= "TANK: "+health;

        //La librería Mathf no permite redondear con decimales, así que a modo de parche se multiplica por 100 la longitud del vector "velocity", se redondea y se divide por 100 de vuelta
        UISpeed.text = "Speed: " + (Mathf.Round(rb.velocity.magnitude*100)/100);    
    



        if(!repairing)
        {
            if(Input.GetKeyDown(KeyCode.R)){
                //No se efectuarán reparaciones si estas no son necesarias
                if(frontArmor==maxFrontArmor && engineArmor==maxEngineArmor && lTrackArmor==maxTrackArmor && rTrackArmor==maxTrackArmor){}
                else{StartCoroutine("Repair");}
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.R)){
                abort=true;
            }
        }


        if(repairing && !abort)
        {
            UIRepair.enabled=true;
        }

        else{
            UIRepair.enabled=false;
        }
    
    }


    void FixedUpdate()
    {
        //El tanque solo responderá si no se está reparando. Si se está reparando la única interacción disponible será la cancelación de las reparaciones
        if(!repairing)
        {
            if(Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W)){
                Forward();
            }
            else if(Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S)){
                Backward();
            }
            if(Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D)){
                RotateRight();
            }
            else if(Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A)){
                RotateLeft();
            }            
        }
                
        //Debug.Log("Current Speed: "+ rb.velocity.magnitude);
        //Debug.Log("Salud: "+health+ " "+frontArmor+ " "+engineArmor+ " "+lTrackArmor+ " "+rTrackArmor);

    }

    //Si un tanque pesado colisiona contra un enemigo u objeto destructible con menor masa se le inflige un daño equivalente a la diferencia entre las masas multiplicado por 10

    //Para mantenerlo a un nivel relativamente razonable se le aplica un pequeño cooldown. Se emplea un Enter en lugar de un Stay debido a que por el movimiento de los tanques enemigos no se detectan bien estos últimos. NOTA: debido a la abrumadora presencia enemiga se estima que no es necesario el cooldown

    private void OnCollisionEnter2D(Collision2D other) {
        if(heavy){
            Rigidbody2D rbo = other.gameObject.GetComponent<Rigidbody2D>();
            if((other.gameObject.tag=="Enemy" || other.gameObject.tag=="Breakable") && !colCool){
                //StartCoroutine("ColCool");
                other.gameObject.SendMessage("Damage",(rb.mass-rbo.mass)*10);                
            }
        }
    }
    /*
    IEnumerator ColCool(){
        colCool=true;
        yield return new WaitForSeconds(.2f);
        colCool=false;
    }
    */


    //Métodos de manejo de movimiento

    void Forward(){
        Vector2 direction= transform.up;
        if(rb.velocity.magnitude<maxSpeed){
            rb.AddForce(direction*currentAcceleration*Time.deltaTime,ForceMode2D.Impulse);
            }
    }

    void Backward(){        
        Vector2 direction= -transform.up;
        if(rb.velocity.magnitude<maxSpeed){
            rb.AddForce(direction*currentAcceleration/4*Time.deltaTime,ForceMode2D.Impulse);
            }
    }


    //Métodos de manejo de rotación

    void RotateRight()
    {
        Vector2 direction=-transform.right;
        rb.AddTorque(-rotSpeed);
    }

    void RotateLeft()
    {
        Vector2 direction=transform.right;
        rb.AddTorque(rotSpeed);
    }



    //Método de manejo de daños
    
    /*Se emplea un sistema que, además de la salud general del tanque, incluye 4 modulos con variables de salud propias y que determinan varios comportamientos

    1. Armadura frontal (etiqueta "Player"): absorberá todo el daño siempre que le quede salud. En caso contrario el daño pasa a la salud general del tanque
    2. Orugas: la oruga impactada absorberá 3/4 del daño en todo momento, pero en caso de quedarse sin salud quedará inutilizada y se aplicará una penalización del 50% a la velocidad. En caso de quedar ambas inutilizadas el tanque no podrá moverse y deberá ser reparado
    3. Motor: funcionamiento similar al de la armadura frontal, pero en caso de quedarse sin salud todo impacto recibido infligirá daño crítico (x2)

    */

    void Damage(object[] response){
        float damage = (float) response[0];
        string part = (string) response[1];

        if(part=="Player"){
            if(frontArmor>0){
                if(frontArmor-damage>0){
                    frontArmor-=damage;
                }
                else{
                    frontArmor=0;
                }
            }
            else{
                if(health-damage>0){
                    health-=damage;
                }
                else{
                    health=0;
                    Die();
                }
            }
            //Debug.Log("Recibido "+damage+ " de daño en el frontal");
        }

        if(part=="Engine"){
            if(engineArmor>0){
                if(engineArmor-damage>0){
                    engineArmor-=damage;
                }
                else{
                    engineArmor=0;
                }
            }
            else{
                if(health-(damage*2)>0){
                    health-=(damage*2);
                }
                else{
                    health=0;
                    Die();
                }
            }
            //Debug.Log("Recibido "+damage+ " de daño en el motor");
        }

        if(part=="LeftTrack"){
            if(lTrackArmor-(damage*.75f)>0){
                lTrackArmor-=(damage*.75f);                    
            }
            else{                
                if(!oneTrack){
                    oneTrack=true;
                    lTrackArmor=0;
                }
                if(rTrackArmor==0){
                    noTrack=true;
                    lTrackArmor=0;
                }
            }

            if(health-(damage*.25f)>0){
                health-=(damage*.25f);
            }
            else{
                health=0;
                Die();
            }
            //Debug.Log("Recibido "+damage+ " de daño en la oruga izquierda");
        }

        if(part=="RightTrack"){            
            if(rTrackArmor-(damage*.75f)>0){
                rTrackArmor-=(damage*.75f);                    
            }
            else{                
                if(!oneTrack){
                    oneTrack=true;
                    rTrackArmor=0;
                }
                if(lTrackArmor==0){
                    noTrack=true;
                    rTrackArmor=0;
                }
            }

            if(health-(damage*.25f)>0){
                health-=(damage*.25f);
            }
            else{
                health=0;
                Die();
            }
            //Debug.Log("Recibido "+damage+ " de daño en la oruga derecha");
        }        
    }


    //Corrutina de manejo de reparación

    //El jugador puede detener el funcionamiento del tanque para efectuar reparaciones en la armadura de los distintos módulos (la salud general no se puede regenerar). Mientras se efectúan las reparaciones no se moverán ni el tanque ni la torreta ni tampoco se podrá disparar. Las reparaciones durarán hasta que todos los módulos de armadura recuperen toda su salud o hasta que el jugador las cancele. La corrutina se itera cada segundo, y en cada iteración se repara un 10% de los puntos de armadura máximos de cada módulo.

    IEnumerator Repair(){
        if(!abort){repairing=true;}        
        while(repairing && !abort){

            frontArmor=Mathf.Min(frontArmor+=maxFrontArmor/10,maxFrontArmor);
            engineArmor=Mathf.Min(engineArmor+=maxEngineArmor/10,maxEngineArmor);
            lTrackArmor=Mathf.Min(lTrackArmor+=maxTrackArmor/10,maxTrackArmor);
            rTrackArmor=Mathf.Min(rTrackArmor+=maxTrackArmor/10,maxTrackArmor);

            if(frontArmor==maxFrontArmor && engineArmor==maxEngineArmor && lTrackArmor==maxTrackArmor && rTrackArmor==maxTrackArmor){
                repairing=false;
            }
            else{yield return new WaitForSeconds(1);}            
        }
        abort=false;
        repairing=false;
    }


    //Método de manejo de muerte

    void Die(){        
        if(!ded){
            ded=true;            
            GameObject.FindGameObjectWithTag("Respawn").SendMessage("BuyTank");
            Instantiate(death, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
            }        
    }


}
