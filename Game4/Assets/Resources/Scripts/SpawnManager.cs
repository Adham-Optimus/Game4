using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Vector3[] firstSpawnPoints;
    public Vector3[] secondSpawnPoints;
    [SerializeField] private GameObject[] enemies;
    private bool canSpawn = true;
    private int canUpHp;
    private void Start()
    {
    }
    private void FixedUpdate()
    {
        if (canSpawn)
        {
            StartCoroutine(SpawnTheEnemy());
        }   
    }
    private IEnumerator SpawnTheEnemy()
    {
        canSpawn = false;
        Vector3[] temp = new Vector3[5];
        if (Random.Range(1,3) == 1)
        {
            temp = firstSpawnPoints;
        }
        else
        {
            temp = secondSpawnPoints;
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            if (canUpHp < 7)
            {
                enemies[i].GetComponent<EnemyStats>().enemyHealth += 1;
                canUpHp++;
            }
            EnemyInstantiate(temp[i], enemies[i]);
        }
        yield return new WaitForSeconds(20);
        canSpawn = true;
    }

    private void EnemyInstantiate(Vector3 _spawn, GameObject g)
    {
        Instantiate(g, _spawn, Quaternion.identity);
    }
}
