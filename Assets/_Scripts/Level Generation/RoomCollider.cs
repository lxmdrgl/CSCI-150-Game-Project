using UnityEngine;


public class RoomCollider : MonoBehaviour
{

    public bool hasCollision = false;
    private PolygonCollider2D polyCollider;

    void Awake()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        polyCollider.enabled = false; // Disable collider initially
        resizeCollider();
    }

    void resizeCollider() {
        float tileSize = 0.5f;

        Vector2[] points = polyCollider.points;
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

        polyCollider.points = newPoints; // Apply new collider shape
        polyCollider.enabled = true; // reenable collision after resize
    }

    public void tryCollider()
    {
        // int roomLayer = LayerMask.NameToLayer("Room");
        // int layerMask = 1 << roomLayer; // Convert layer to bitmask
        
        // Collider2D[] results = Physics2D.OverlapBoxAll(transform.position, boxCollider.bounds.size, 0, layerMask);
        // int numOverlap = results.Length;
        // Debug.Log("tryCollider(): " + gameObject.transform.parent);
        // foreach (Collider2D collider in results) {
        //     if (collider != boxCollider) {
        //         Debug.LogError("Collision detected: " + gameObject.transform.parent + " collision with " + collider.gameObject.transform.parent);
        //     }
        // }

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
