using UnityEngine;
using Game.CoreSystem; 

public class SimpleDamage : MonoBehaviour
{
    public float damageAmount = 10f; // Damage dealt by the projectile

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with the Player
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            // Retrieve the Stats component from the Player's Core
            Stats stats = player.Core.GetCoreComponent<Stats>();
            if (stats != null)
            {
                // Reduce the player's health using the Stats component
                stats.Health.Decrease(damageAmount);

                /* Trigger the DeadState if health is zero or below
                if (stats.Health.CurrentValue <= 0)
                {
                    player.StateMachine.ChangeState(player.DeadState);
                }*/
            }

            // Destroy the projectile after dealing damage
            Destroy(gameObject);
        }
    }
}
