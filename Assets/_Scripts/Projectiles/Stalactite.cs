using UnityEngine;
using Game.Combat.Damage;
using Game.Utilities;

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
        Collider2D[] colliders = new Collider2D[1] { collision };

        if (collision.CompareTag("Player"))
        {
            CombatDamageUtilities.TryDamage(colliders, new DamageData(damage, gameObject), out _);
            Destroy(gameObject);
        }
        else if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
