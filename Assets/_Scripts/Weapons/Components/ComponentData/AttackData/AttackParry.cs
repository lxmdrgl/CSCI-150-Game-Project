using System;
using UnityEngine;

namespace Game.Weapons.Components
{
    [Serializable]
    public class AttackParry : AttackData
    {
        public bool Debug;
        [field: SerializeField] public PolygonCollider2D HitBox { get; private set; }
    }
}