using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnController : MonoBehaviour
{    

    [SerializeField] GameObject enemy;
    int spawns=0;

    void Start()
    {
        StartCoroutine("EnemySpawn");
    }


    IEnumerator EnemySpawn(){        
        while(spawns<GameManager.enemyWaves[SceneManager.GetActiveScene().buildIndex]){
            Instantiate(enemy, transform.position,transform.rotation);
            spawns++;
            yield return new WaitForSeconds(5);
        }
    }
}
