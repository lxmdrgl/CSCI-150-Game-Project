using System;
using Game.Projectiles;
using UnityEngine;
using System.Collections.Generic;

namespace Game.Weapons.Components
{
    [Serializable]
    public class AttackProjectileSpawnerCharge : AttackData
    {
        [field: SerializeField] public List<AttackProjectileSpawner> ProjectileSpawners { get; private set; }
    }
}