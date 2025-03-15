using System;
using UnityEngine;

namespace Game.Weapons.Components
{
    [Serializable]
    public class AttackFall : AttackData
    {
        [field: SerializeField] public Vector2 Angle { get; private set; }
        [field: SerializeField] public float Velocity { get; private set; }
    }
}