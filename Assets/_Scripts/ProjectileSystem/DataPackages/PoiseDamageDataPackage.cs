using System;
using UnityEngine;

namespace Game.ProjectileSystem.DataPackages
{
    [Serializable]
    public class StunDamageDataPackage : ProjectileDataPackage
    {
        [field: SerializeField] public float Amount { get; private set; }
    }
}