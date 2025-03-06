using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[System.Serializable]
public class RoomManager : MonoBehaviour
{
    public GameObject gameObj;
    public List<GameObject> entrances;
    public List<GameObject> exits;
    public List<Collider2D> colliders;

    void OnEnable() {
        PopulateVars();
    }

    void OnValidate() {
        PopulateVars();
    }

    void PopulateVars()
    {
        entrances.Clear();
        exits.Clear();

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
            
            Collider2D collider = child.GetComponent<Collider2D>();
            if (collider != null)
            {
                colliders.Add(collider);
            }
        }
    }

}
