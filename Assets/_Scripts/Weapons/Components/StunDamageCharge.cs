// using Game.Interfaces;
using UnityEngine;

using Game.Combat.StunDamage;
using Game.CoreSystem;
using static Game.Utilities.StunDamageUtilities;

namespace Game.Weapons.Components
{
    public class StunDamageCharge : WeaponComponent<StunDamageChargeData, AttackStunDamageCharge>
    {
        private ActionHitBox hitBox;

        private Charge charge;
        private Stats stats;
        private int chargeAmount;

        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            if (charge != null) {
                chargeAmount = charge.TakeFinalChargeReading() - 1;
            } if (chargeAmount < currentAttackData.Amounts.Count) {
                float stunAmount = currentAttackData.Amounts[chargeAmount] * (stats.Attack / 100f); 
                TryStunDamage(colliders, new Combat.StunDamage.StunDamageData(stunAmount, Core.Root), out _);
            }

            // Debug.Log("Detected Collider for Stun");
            // foreach (var item in colliders)
            // {
            //     if (item.TryGetComponent(out IStunDamageable stunDamageable))
            //     {
            //         stunDamageable.DamageStun(new Combat.StunDamage.StunDamageData(currentAttackData.Amounts[chargeAmount], Core.Root));

            //         Debug.Log("Stun Damage Delt");
            //     }
            // }
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