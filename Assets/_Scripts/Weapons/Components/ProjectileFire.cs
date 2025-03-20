using UnityEngine;

using Game.Combat.Damage;
using static Game.Utilities.CombatDamageUtilities;
using Game.CoreSystem;
using System.Runtime.CompilerServices;
using JetBrains.Annotations; //(2)

using Game.Projectiles;
using System.Collections;


namespace Game.Weapons.Components
{
    public class ProjectileFire : WeaponComponent<ProjectileFireData, AttackProjectileFire>
    {
        private ProjectileSpawner projectileSpawner;
        private Stats stats;
        private Game.CoreSystem.Movement movement;

        private void HandleSpawnProjectile(Projectile projectile)
        {
            // currentAttackData.attack = stats.Attack;
            
            projectile.FireProjectile(currentAttackData, stats.Attack, movement.FacingDirection);
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