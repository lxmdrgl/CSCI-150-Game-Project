using System.Collections.Generic;
using UnityEngine;

using Game.Combat.Status;

namespace Game.Utilities
{
    /*
     * This Utility class provides some static functions for logic we might perform in many different places. This way
     * we can keep it all consolidated here and only have to change it in one place. That is the dream anyway.
     *
     * For example: The Damage functions are called by both DamageOnHitBoxAction and DamageOnBlock weapon components.
     */
    public static class CombatStatusDamageUtilities
    {
        public static bool TryStatus(GameObject gameObject, StatusData statusData, out IStatusDamageable damageable)
        {
            // Debug.Log("Enter TryStatus");
            // TryGetComponentInChildren is a custom GameObject extension method.
            if (gameObject.TryGetComponentInChildren(out damageable))
            {
                damageable.StatusDamage(statusData);
                return true;
            }

            return false;
        }

        public static bool TryStatus(Collider2D[] colliders, StatusData statusData, out List<IStatusDamageable> damageables)
        {
            var hasDamaged = false;
            damageables = new List<IStatusDamageable>();
            
            foreach (var collider in colliders)
            {
                if (TryStatus(collider.gameObject, statusData, out IStatusDamageable damageable))
                {
                    damageables.Add(damageable);
                    hasDamaged = true;
                }
            }

            return hasDamaged;
        }
    }
}