using NUnit.Framework;
using UnityEngine;


public class RoomCollider : MonoBehaviour
{
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

        // Check if any point has non-whole-number coordinates
        bool hasNonWholeNumbers = false;
        foreach (Vector2 point in points) {
            if (point.x % 1 != 0 || point.y % 1 != 0) {
                hasNonWholeNumbers = true;
                break;
            }
        }

        // If any point is not a whole number, keep the collider unchanged
        if (hasNonWholeNumbers) {
            // Debug.Log("Skipping collider resize: Awake called twice");
            polyCollider.points = points;
            return;
        }

        // Compute inward normals and move each point
        for (int i = 0; i < numPoints; i++) {
            Vector2 prev = points[(i - 1 + numPoints) % numPoints]; // Wrap around
            Vector2 next = points[(i + 1) % numPoints];

            Vector2 edge1 = (points[i] - prev).normalized;
            Vector2 edge2 = (next - points[i]).normalized;

            Vector2 normal1 = new Vector2(-edge1.y, edge1.x);
            Vector2 normal2 = new Vector2(-edge2.y, edge2.x);

            Vector2 inwardNormal = (normal1 + normal2).normalized;

            newPoints[i] = points[i] + inwardNormal * -tileSize;
        }

        polyCollider.points = newPoints; // Apply new collider shape

        // Debug.Log("Collision resized for: " + gameObject.transform.parent);
    }

    // public void tryCollider() {
    //     hasCollision = false; // Reset collision flag

    //     // Create a ContactFilter2D to filter only the "Room" layer
    //     ContactFilter2D filter = new ContactFilter2D();
    //     filter.SetLayerMask(LayerMask.GetMask("Room"));
    //     filter.useTriggers = true;

    //     // Check for overlapping colliders
    //     Collider2D[] results = new Collider2D[10]; // Buffer for results
    //     int collisionCount = polyCollider.Overlap(filter, results);

    //     if (collisionCount > 0)
    //     {
    //         hasCollision = true;
    //     }
    //     Debug.Log("Number of collisions detected on: " + gameObject.transform.parent + ", " + collisionCount);
    // }

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
