using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.CoreSystem;
using Game.Projectiles;
using Unity.VisualScripting.FullSerializer.Internal;

namespace Game.Weapons.Components
{
    public class ProjectileSpawner : WeaponComponent<ProjectileSpawnerData, AttackProjectileSpawner>
    {
        public event Action<Projectile> OnSpawnProjectile;
        private Game.CoreSystem.Movement movement;

        private void HandleProjectileAction()
        {
            StartCoroutine(SpawnProjectilesCoroutine());
        }

        private IEnumerator SpawnProjectilesCoroutine()
        {
            float nextSpawnTime = Time.time;

            for (int i = 0; i < currentAttackData.count; i++)
            {
                SpawnProjectile();

                // Calculate the next spawn time
                nextSpawnTime += currentAttackData.timeInterval;

                // Wait until the next spawn time
                while (Time.time < nextSpawnTime)
                {
                    yield return null;
                }
            }
        }

        private void SpawnProjectile()
        {
            Vector3 spawnPosition = transform.position + new Vector3(currentAttackData.position.x * movement.FacingDirection, currentAttackData.position.y, 0f);
            Projectile newProjectile = Instantiate(currentAttackData.projectile, spawnPosition, Quaternion.identity);
            Debug.Log("Spawnposition: " + spawnPosition + ", " + transform.position);

            OnSpawnProjectile?.Invoke(newProjectile);
        }

        protected override void HandleExit()
        {

        }

        protected override void Start()
        {
            base.Start();
            movement = Core.GetCoreComponent<Game.CoreSystem.Movement>();
            AnimationEventHandler.OnProjectileAction += HandleProjectileAction;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            AnimationEventHandler.OnProjectileAction -= HandleProjectileAction;
        }
    }
}