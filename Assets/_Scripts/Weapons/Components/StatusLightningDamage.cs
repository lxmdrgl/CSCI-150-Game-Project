// using Game.Interfaces;
using UnityEngine;

using Game.CoreSystem;
using static Game.Utilities.CombatStatusDamageUtilities;
using Game.Combat.Status;

namespace Game.Weapons.Components
{
    public class StatusLightningDamage : WeaponComponent<StatusLightningDamageData, AttackStatusLightningDamage>
    {
        private ActionHitBox hitBox;
        private Stats stats;

        protected virtual void HandleDetectCollider2D(Collider2D[] colliders)
        {
            LightningStatus newStatus = new LightningStatus(currentAttackData.Amount,
                                                currentAttackData.Damage * (stats.Attack/100f),
                                                currentAttackData.Stun * (stats.Attack/100f),
                                                Core.Root,
                                                currentAttackData.Ticks,
                                                currentAttackData.Delay,
                                                currentAttackData.Count,
                                                currentAttackData.Radius,
                                                currentAttackData.WhatIsDamageable);
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