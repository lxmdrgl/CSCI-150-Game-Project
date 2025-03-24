// using Game.Interfaces;
using UnityEngine;

using Game.CoreSystem;
using static Game.Utilities.CombatStatusDamageUtilities;
using Game.Combat.Status;

namespace Game.Weapons.Components
{
    public class StatusFireDamage : WeaponComponent<StatusFireDamageData, AttackStatusFireDamage>
    {
        private ActionHitBox hitBox;
        private Stats stats;

        protected virtual void HandleDetectCollider2D(Collider2D[] colliders)
        {
            FireStatus newStatus = new FireStatus(currentAttackData.Amount,
                                                currentAttackData.Damage * (stats.Attack/100f),
                                                currentAttackData.Stun * (stats.Attack/100f),
                                                Core.Root,
                                                currentAttackData.Ticks,
                                                currentAttackData.Delay,
                                                currentAttackData.Mult);

            // Debug.Log("TryStatus");
            TryStatus(colliders, newStatus, out _);
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