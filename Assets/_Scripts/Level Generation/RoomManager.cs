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

            PolygonCollider2D collider = child.GetComponent<PolygonCollider2D>();
            if (collider != null)
            {
                roomCollider = collider;
            }
        }
    }

    public void SpawnEnemies()
    {
        foreach(GameObject enemySpawner in enemySpawners) 
        {
            GameObject enemy =  Instantiate(enemies[Random.Range(0, enemies.Count)], enemySpawner.transform.position, Quaternion.identity);
            enemy.GetComponent<Entity>().GenerateGuid();
        }
    }
}
