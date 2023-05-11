using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    void Start()
    {        
        StartCoroutine("Suicide");
    }

    IEnumerator Suicide(){
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
}
