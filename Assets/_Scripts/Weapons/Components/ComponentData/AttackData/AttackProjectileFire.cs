using System;
using Game.Combat.Status;
using Game.Projectiles;
using UnityEngine;

namespace Game.Weapons.Components
{
    [Serializable]
    public class AttackProjectileFire : AttackData
    {
        [field: SerializeField] public float velocity { get; private set; }
        [field: SerializeField] public Vector2 direction { get; private set; }
        [field: SerializeField] public bool rotate { get; private set; }
        [field: SerializeField] public bool hasGravity { get; private set; }
        [field: SerializeField] public float gravityScale { get; private set; }
        [field: SerializeField] public float damage { get; private set; }
        [field: SerializeField] public float stun { get; private set; }

        [field: SerializeField] public bool pierce { get; private set; }
        [field: SerializeField] public bool explosive { get; private set; }
        [field: SerializeField] public float explosiveRadius { get; private set; }
        [field: SerializeField] public bool target { get; private set; }
        [field: SerializeField] public float targetRadius { get; private set; }
        
        [field: SerializeField] public LayerMask whatIsGround { get; private set; }
        [field: SerializeField] public LayerMask whatIsDamageable { get; private set; }
        public StatusData statusData { get; private set;}

        // [field: SerializeField] public D_FireStatus FireStatusData { get; private set; }
    }
}