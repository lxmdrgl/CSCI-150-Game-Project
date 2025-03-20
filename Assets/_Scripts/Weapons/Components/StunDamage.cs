// using Game.Interfaces;
using UnityEngine;

using Game.Combat.StunDamage;
using Game.CoreSystem;
using static Game.Utilities.StunDamageUtilities;

namespace Game.Weapons.Components
{
    public class StunDamage : WeaponComponent<StunDamageData, AttackStunDamage>
    {
        private ActionHitBox hitBox;
        private Stats stats;

        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            // Debug.Log("Detected Collider for Stun");
            // foreach (var item in colliders)
            // {
            //     if (item.TryGetComponent(out IStunDamageable stunDamageable))
            //     {
            //         stunDamageable.DamageStun(new Combat.StunDamage.StunDamageData(currentAttackData.Amount, Core.Root));
            //         Debug.Log("Stun Damage Delt");
            //     }
            // }
            float stunAmount = currentAttackData.Amount * (stats.Attack / 100f);

            TryStunDamage(colliders, new Combat.StunDamage.StunDamageData(stunAmount, Core.Root), out _);
        }
        
        protected override void Start()
        {
            base.Start();

            hitBox = GetComponent<ActionHitBox>();
            stats = Core.GetCoreComponent<Stats>();

            hitBox.OnDetectedCollider2D += HandleDetectCollider2D;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;
        }
    }
}