using UnityEngine;

public class Stalactite : MonoBehaviour
{
    public float damage = 10f;
    public float destroyDelay = 3f;
    public LayerMask groundLayer;

    private void Start()
    {
        Destroy(gameObject, destroyDelay); // fallback if nothing is hit
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            Destroy(gameObject);
        }
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
