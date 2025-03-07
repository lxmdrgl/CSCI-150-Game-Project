using UnityEngine;

public class RoomCollider : MonoBehaviour
{

    public bool hasCollision = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        hasCollision = true;
    }
}
