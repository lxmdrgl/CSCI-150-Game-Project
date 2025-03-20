using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
using Game.Projectiles;
using Unity.VisualScripting.FullSerializer.Internal;

namespace Game.Weapons.Components
{
    public class ProjectileSpawnerCharge : WeaponComponent<ProjectileSpawnerChargeData, AttackProjectileSpawnerCharge>
    {
        public event Action<Projectile> OnSpawnProjectile;
        private Game.CoreSystem.Movement movement;
        private Charge charge;
        private int chargeAmount;
        

        private void HandleProjectileAction()
        {

            if (charge != null) {
                chargeAmount = charge.TakeFinalChargeReading() - 1;
            } 
            StartCoroutine(SpawnProjectilesCoroutine(currentAttackData.ProjectileSpawners[chargeAmount]));
        }

        private IEnumerator SpawnProjectilesCoroutine(AttackProjectileSpawner spawner)
        {
            float nextSpawnTime = Time.time;

            for (int i = 0; i < spawner.count; i++)
            {
                SpawnProjectile(spawner);

                // Calculate the next spawn time
                nextSpawnTime += spawner.timeInterval;

                // Wait until the next spawn time
                while (Time.time < nextSpawnTime)
                {
                    yield return null;
                }
            }
        }

        private void SpawnProjectile(AttackProjectileSpawner spawner)
        {
            Vector3 spawnPosition = transform.position + 
                                    new Vector3(spawner.position.x * movement.FacingDirection, spawner.position.y, 0f);
            
            Projectile newProjectile = Instantiate(spawner.projectile, spawnPosition, Quaternion.identity);

            OnSpawnProjectile?.Invoke(newProjectile);
        }

        protected override void HandleExit()
        {

        }

        protected override void Start()
        {
            base.Start();
            movement = Core.GetCoreComponent<Game.CoreSystem.Movement>();
            charge = GetComponent<Charge>();
            AnimationEventHandler.OnProjectileAction += HandleProjectileAction;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AnimationEventHandler.OnProjectileAction -= HandleProjectileAction;
        }
    }
}