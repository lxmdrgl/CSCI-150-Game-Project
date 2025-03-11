using UnityEngine;

public class RoomCollider : MonoBehaviour
{

    public bool hasCollision = false;

    void OnEnable()
    {
        int roomLayer = LayerMask.NameToLayer("Room");
        int layerMask = 1 << roomLayer; // Convert layer to bitmask

        Collider2D hit = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().bounds.size, 0, layerMask);

        if (hit != null && hit.gameObject != gameObject)
        {
            hasCollision = true;
            // Debug.Log("Collision detected with a 'Room' object: " + hit.gameObject.name);
        }
    }
}
