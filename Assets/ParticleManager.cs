using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public float timeToDestroy = 0.2f;
    public float speed = 0f;
    public Vector2 positionOffset = Vector2.zero;

    void Start()
    {
        transform.position = new Vector2(transform.position.x + positionOffset.x, transform.position.y + positionOffset.y);
        
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}
