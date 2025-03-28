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
    public class ProjectileFire : WeaponComponent<ProjectileFireData, AttackProjectileFire>
    {
        private ProjectileSpawner projectileSpawner;
        private Stats stats;
        private Game.CoreSystem.Movement movement;
        private StatusData statusData;

        private void HandleSpawnProjectile(Projectile projectile)
        {
            projectile.FireProjectile(currentAttackData, stats.Attack, movement.FacingDirection, statusData, Core.Root);
            if (statusData != null)
            {
                // statusData = null;
            }
            else 
            {
                Debug.Log("StatusData is null");
            }
        }

        public void SetStatusData(StatusData statusData)
        {
            this.statusData = statusData;
        }

        protected override void Start()
        {
            base.Start();
            stats = Core.GetCoreComponent<Stats>();
            movement = Core.GetCoreComponent<Game.CoreSystem.Movement>();
            projectileSpawner = GetComponent<ProjectileSpawner>();
            
            projectileSpawner.OnSpawnProjectile += HandleSpawnProjectile;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            projectileSpawner.OnSpawnProjectile -= HandleSpawnProjectile;
        }
    }
}