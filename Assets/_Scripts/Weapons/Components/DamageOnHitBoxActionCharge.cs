using UnityEngine;

using Game.Combat.Damage;
using static Game.Utilities.CombatDamageUtilities;
using Game.CoreSystem;
using System.Runtime.CompilerServices;
using JetBrains.Annotations; //(2)

namespace Game.Weapons.Components
{
    public class DamageOnHitBoxActionCharge : WeaponComponent<DamageOnHitBoxActionChargeData, AttackDamageCharge>
    {
        private ActionHitBox hitBox;

        private Stats stats;
        private Charge charge;
        
        private float totalAmount;

        private int chargeAmount;

        private void HandleDetectCollider2D(Collider2D[] colliders)
        {
            // Notice that this is equal to (1), the logic has just been offloaded to a static helper class. Notice the using statement (2) is static, allowing as to call the Damage function directly instead of saying
            // Game.Utilities.CombatUtilities.Damage(...);

            if (charge != null) {
                chargeAmount = charge.TakeFinalChargeReading() - 1;
            } 
            if (chargeAmount < currentAttackData.Amounts.Count) {
                totalAmount = currentAttackData.Amounts[chargeAmount] * (stats.Attack / 100f); 
            }

            // Ensure TryDamage is called only once
            TryDamage(colliders, new DamageData(totalAmount, Core.Root), out _); 
            Debug.Log("Charge damage amount: " + totalAmount);
        }

        protected override void Start()
        {
            base.Start();

            hitBox = GetComponent<ActionHitBox>();
            
            hitBox.OnDetectedCollider2D += HandleDetectCollider2D;

            stats = Core.GetCoreComponent<Stats>();

            charge = GetComponent<Charge>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnDetectedCollider2D -= HandleDetectCollider2D;
        }
    }
}