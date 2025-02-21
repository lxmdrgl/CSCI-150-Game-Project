using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Game.Weapons.Components;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Weapon Data Set", order = 0)]
    public class WeaponDataSet : ScriptableObject
    {
        [field: SerializeField] public PlayerType playerType { get; private set; }
        [field: SerializeField] public List<WeaponData> weaponData { get; private set; }
    }
}