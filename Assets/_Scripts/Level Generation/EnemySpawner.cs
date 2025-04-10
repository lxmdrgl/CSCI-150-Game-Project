using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> possible_enemies;

    public void SpawnEnemy()
    {
        GameObject enemy =  Instantiate(possible_enemies[Random.Range(0, possible_enemies.Count)], transform.position, Quaternion.identity);
        enemy.GetComponent<Entity>().GenerateGuid();
    }
}
