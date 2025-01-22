// using Game.Interfaces;
using UnityEngine;

using Game.Combat.StunDamage;
using static Game.Utilities.StunDamageUtilities;

namespace Game.Weapons.Components
{
    public class StunDamage : WeaponComponent<StunDamageData, AttackStunDamage>
    {
        private ActionHitBox hitBox;

        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            Debug.Log("Detected Collider for Stun");
            foreach (var item in colliders)
            {
                if (item.TryGetComponent(out IStunDamageable stunDamageable))
                {
                    stunDamageable.DamageStun(new Combat.StunDamage.StunDamageData(currentAttackData.Amount, Core.Root));
                    Debug.Log("Stun Damage Delt");
                }
            }

            TryStunDamage(colliders, new Combat.StunDamage.StunDamageData(currentAttackData.Amount, Core.Root), out _);
        }
        
        protected override void Start()
        {
            base.Start();

            hitBox = GetComponent<ActionHitBox>();

            hitBox.OnDetectedCollider2D += HandleDetectCollider2D;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;
        }
    }
}