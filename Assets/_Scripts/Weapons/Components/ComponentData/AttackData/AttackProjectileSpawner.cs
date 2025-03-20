using System;
using Game.Projectiles;
using UnityEngine;

namespace Game.Weapons.Components
{
    [Serializable]
    public class AttackProjectileSpawner : AttackData
    {
        [field: SerializeField] public Projectile projectile { get; private set; }
        [field: SerializeField] public Vector2 position { get; private set; }
        [field: SerializeField] public int count { get; private set; }
        [field: SerializeField] public float timeInterval { get; private set; }
    }
}