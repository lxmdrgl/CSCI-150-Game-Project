using System.Collections.Generic;
using UnityEngine;

using Game.Combat.StunDamage;

namespace Game.Utilities
{
    /*
     * This Utility class provides some static functions for logic we might perform in many different places. This way
     * we can keep it all consolidated here and only have to change it in one place. That is the dream anyway.
     *
     * For example: The Damage functions are called by both DamageOnHitBoxAction and DamageOnBlock weapon components.
     */
    public static class StunDamageUtilities
    {
        public static bool TryStunDamage(GameObject gameObject, StunDamageData stunDamageData, out IStunDamageable damageable)
        {
            // TryGetComponentInChildren is a custom GameObject extension method.
            if (gameObject.TryGetComponentInChildren(out damageable))
            {
                damageable.DamageStun(stunDamageData);
                return true;
            }

            return false;
        }

        public static bool TryStunDamage(Collider2D[] colliders, StunDamageData stunDamageData, out List<IStunDamageable> damageables)
        {
            var hasDamaged = false;
            damageables = new List<IStunDamageable>();
            
            foreach (var collider in colliders)
            {
                if (TryStunDamage(collider.gameObject, stunDamageData, out IStunDamageable damageable))
                {
                    damageables.Add(damageable);
                    hasDamaged = true;
                }
            }

            return hasDamaged;
        }
    }
}