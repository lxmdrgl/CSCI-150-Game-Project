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
    public List<Entity> enemies;
    public List<GameObject> enemySpawners;

    void Awake() {
        PopulateVars();
        ResizeCollider();
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

    void ResizeCollider() {
        float tileSize = 0.5f;

        Vector2[] points = roomCollider.points;
        int numPoints = points.Length;
        Vector2[] newPoints = new Vector2[numPoints];

        // Compute inward normals and move each point
        for (int i = 0; i < numPoints; i++)
        {
            // Get the previous and next point to determine the normal
            Vector2 prev = points[(i - 1 + numPoints) % numPoints]; // Wrap around
            Vector2 next = points[(i + 1) % numPoints];

            // Edge vectors
            Vector2 edge1 = (points[i] - prev).normalized;
            Vector2 edge2 = (next - points[i]).normalized;

            // Compute an inward normal (perpendicular to edges)
            Vector2 normal1 = new Vector2(-edge1.y, edge1.x);
            Vector2 normal2 = new Vector2(-edge2.y, edge2.x);

            // Average normals for smooth inward movement
            Vector2 inwardNormal = (normal1 + normal2).normalized;

            // Move the point inward
            newPoints[i] = points[i] + inwardNormal * -tileSize;
        }

        roomCollider.points = newPoints; // Apply new collider shape
    }

    public void SpawnEnemies()
    {
        foreach(GameObject enemySpawner in enemySpawners) 
        {
            Entity enemy =  Instantiate(enemies[Random.Range(0, enemies.Count)], enemySpawner.transform.position, Quaternion.identity);
            enemy.GenerateGuid();
        }
    }
}
