using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Game.Weapons.Components;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Ugrade Data/Stat Upgrade Data Set", order = 0)]
    public class StatUpgradeDataSet : ScriptableObject
    {
        // [field: SerializeReference] public PlayerType playerType { get; private set; }
        [field: SerializeReference] public List<StatUpgradeData> statUpgradeData { get; private set; }
    }
}