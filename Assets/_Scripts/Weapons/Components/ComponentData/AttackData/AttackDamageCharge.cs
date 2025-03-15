using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons.Components
{
    [Serializable]
    public class AttackDamageCharge : AttackData
    {
        [field: SerializeField] public List<float> Amounts { get; private set; }
    }
}