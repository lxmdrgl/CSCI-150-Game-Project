using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Game.Weapons.Components;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Ugrade Data/Stat Upgrade Data", order = 0)]
    public class StatUpgradeData : ScriptableObject
    {
        [field: SerializeReference] public float Health { get; private set; }
        [field: SerializeReference] public float Attack { get; private set; }
    }
}