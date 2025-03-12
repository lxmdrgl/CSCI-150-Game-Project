using UnityEngine;

public class RoomCollider : MonoBehaviour
{

    public bool hasCollision = false;
    private BoxCollider2D boxCollider;

    void OnEnable()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        // boxCollider.enabled = false; // Disable collider initially
    }

    public void enableCollider() {
        boxCollider.enabled = true;
    }

    public void tryCollider()
    {
        int roomLayer = LayerMask.NameToLayer("Room");
        int layerMask = 1 << roomLayer; // Convert layer to bitmask
        // List<Collider2D> results = new List<Collider2D>();

        // int numOverlap = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().bounds.size, 0, layerMask, results);

        // if (hit != null) {
        //     Debug.Log("self:" + gameObject.transform.parent + ", hit: " + hit.gameObject.transform.parent);
        // }
        // else {
        //     Debug.Log("null result, self:" + gameObject.transform.parent);
        // } 
        
        // if (hit != null && hit.gameObject != gameObject)
        // {
        //     hasCollision = true;
        //     Debug.Log("Collision detected: " + gameObject.transform.parent + " collision with " + hit.gameObject.transform.parent);
        // }
    }
}
