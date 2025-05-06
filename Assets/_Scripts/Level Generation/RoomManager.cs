using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[System.Serializable]
public class RoomManager : MonoBehaviour
{
    public GameObject gameObj;
    public List<GameObject> entrances;
    public List<GameObject> exits;
    public PolygonCollider2D roomCollider;
    public bool hasCollision = false;
    public List<GameObject> enemies;
    public List<GameObject> enemySpawners;

    void Awake() {
        PopulateVars();
    }

    void OnValidate() {
        PopulateVars();
    }

    void PopulateVars()
    {
        entrances.Clear();
        exits.Clear();
        enemySpawners.Clear();
        roomCollider = null;


        foreach (Transform child in transform)
        {
            if (child.CompareTag("RoomEntrance")) // Ensure entrance objects are tagged properly
            {
                entrances.Add(child.gameObject);
            }
            else if (child.CompareTag("RoomExit")) // Ensure exit objects are tagged properly
            {
                exits.Add(child.gameObject);
            }
            else if (child.CompareTag("EnemySpawner")) // Ensure exit objects are tagged properly
            {
                enemySpawners.Add(child.gameObject);
            }

            PolygonCollider2D collider = child.GetComponent<PolygonCollider2D>();
            if (collider != null)
            {
                roomCollider = collider;
            }
        }
    }

    public void SpawnEnemies(float healthIncrease, float attackIncrease)
    {
        foreach(GameObject enemySpawner in enemySpawners) 
        {
            List<GameObject> possibleEnemies = new List<GameObject>(enemySpawner.GetComponent<EnemySpawner>().possible_enemies);
            GameObject enemy =  Instantiate(possibleEnemies[Random.Range(0, possibleEnemies.Count)], enemySpawner.transform.position, Quaternion.identity);
            Entity entity = enemy.GetComponent<Entity>();
            // enemy.GetComponent<Entity>().GenerateGuid();
            entity.GenerateGuid();
            
            entity.stats.UpdateStats(healthIncrease, attackIncrease);
            entity.stats.Health.Increase(entity.stats.Health.MaxValue);
        }
    }
}
