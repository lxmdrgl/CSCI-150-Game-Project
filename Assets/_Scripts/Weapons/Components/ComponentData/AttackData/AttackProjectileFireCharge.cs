using System;
using System.Collections.Generic;
using Game.Projectiles;
using UnityEngine;

namespace Game.Weapons.Components
{
    [Serializable]
    public class AttackProjectileFireCharge : AttackData
    {
        [field: SerializeField] public List<AttackProjectileFire> ProjectileFires { get; private set; }
    }
}