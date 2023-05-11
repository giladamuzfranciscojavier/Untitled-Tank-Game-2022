using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    
    //El objetivo de este script es asignar la cámara al tanque del jugador y, en caso de que este sea destruido y reemplazado por otro, asignar la cámara al nuevo tanque

    CinemachineVirtualCamera vc;

    void Start()
    {
        vc = GetComponent<CinemachineVirtualCamera>();
        vc.Follow = GameObject.FindWithTag("Player").gameObject.transform;
        vc.LookAt = GameObject.FindWithTag("Player").gameObject.transform;
    }

    void Update()
    {
        //En caso de que se deje de existir el objeto que estaba siguiendo la cámara se buscará otro con la misma etiqueta
        if(vc.Follow==null){
            vc.Follow = GameObject.FindWithTag("Player").gameObject.transform;
            vc.LookAt = GameObject.FindWithTag("Player").gameObject.transform;
        }
    }
}
