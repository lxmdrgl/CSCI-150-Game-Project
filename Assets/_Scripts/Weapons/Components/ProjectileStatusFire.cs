using UnityEngine;

using Game.Combat.Damage;
using static Game.Utilities.CombatDamageUtilities;
using Game.CoreSystem;
using System.Runtime.CompilerServices;
using JetBrains.Annotations; //(2)

using Game.Projectiles;
using System.Collections;
using Game.Combat.Status;


namespace Game.Weapons.Components
{
    public class ProjectileStatusFire : WeaponComponent<ProjectileStatusFireData, AttackStatusFireDamage>
    {
        private ProjectileFire projectileFire;
        private ProjectileFireCharge projectileFireCharge;
        private Stats stats;

        protected override void HandleEnter()
        {
            base.HandleEnter();

            Debug.Log("Projectile status current: " + currentAttackData);
            FireStatus newStatus = new FireStatus(currentAttackData.Amount,
                                                currentAttackData.Damage * (stats.Attack/100f),
                                                currentAttackData.Stun * (stats.Attack/100f),
                                                Core.Root,
                                                currentAttackData.Ticks,
                                                currentAttackData.Delay,
                                                currentAttackData.Mult);
            Debug.Log("Projectile status newStatus: " + newStatus);
            if (projectileFire != null)
            {
                projectileFire.SetStatusData(newStatus);
            }
            if (projectileFireCharge != null)
            {
                projectileFireCharge.SetStatusData(newStatus);
            }
        }

        protected override void Start()
        {
            base.Start();

            stats = Core.GetCoreComponent<Stats>();
            projectileFire = GetComponent<ProjectileFire>();
            projectileFireCharge = GetComponent<ProjectileFireCharge>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}