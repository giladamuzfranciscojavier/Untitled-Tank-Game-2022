using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretController : MonoBehaviour
{
    
    [SerializeField] float rotSpeed;
    
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 direction = player.transform.position - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle-90,Vector3.forward);
        this.transform.rotation =  Quaternion.RotateTowards(this.transform.rotation, rotation,rotSpeed*Time.deltaTime); 
    }
}
