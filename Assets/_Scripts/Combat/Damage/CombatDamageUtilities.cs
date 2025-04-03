using System.Collections.Generic;
using UnityEngine;

using Game.Combat.Damage;
using Unity.VisualScripting;

namespace Game.Utilities
{
    /*
     * This Utility class provides some static functions for logic we might perform in many different places. This way
     * we can keep it all consolidated here and only have to change it in one place. That is the dream anyway.
     *
     * For example: The Damage functions are called by both DamageOnHitBoxAction and DamageOnBlock weapon components.
     */
    public static class CombatDamageUtilities
    {
        public static bool TryDamage(GameObject gameObject, DamageData damageData, out IDamageable damageable)
        {
            // TryGetComponentInChildren is a custom GameObject extension method.
            if (gameObject.TryGetComponentInChildren(out damageable))
            {
                damageable.Damage(damageData);
                return true;
            }

            // If we don't find a damageable component on the gameObject, we need to check the parent transforms.
            Transform currTransform = gameObject.transform;
            Transform oldTransform = currTransform;
            Debug.Log("Start combat damage transform: " + currTransform);
            while (currTransform != null)
            {
                oldTransform = currTransform;
                currTransform = currTransform.parent;
                Debug.Log("Loop combat damage transform: " + currTransform);
            }

            if (oldTransform.gameObject.TryGetComponentInChildren(out damageable))
            {
                Debug.Log("Deal combat damage transform: " + currTransform);
                damageable.Damage(damageData);
                return true;
            }

            return false;
        }

        public static bool TryDamage(Collider2D[] colliders, DamageData damageData, out List<IDamageable> damageables)
        {
            var hasDamaged = false;
            damageables = new List<IDamageable>();
            
            foreach (var collider in colliders)
            {
                if (TryDamage(collider.gameObject, damageData, out IDamageable damageable))
                {
                    damageables.Add(damageable);
                    hasDamaged = true;
                }
            }

            return hasDamaged;
        }
    }
}