using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawnController : MonoBehaviour
{    

    [SerializeField] GameObject enemy;

    void Start()
    {
        StartCoroutine("EnemySpawn");
    }


    IEnumerator EnemySpawn(){

        int spawns=0;

        while(spawns<GameManager.enemySpawns[SceneManager.GetActiveScene().buildIndex]){
            Instantiate(enemy, transform.position,transform.rotation);
            spawns++;
            yield return new WaitForSeconds(5);
        }
    }
}
