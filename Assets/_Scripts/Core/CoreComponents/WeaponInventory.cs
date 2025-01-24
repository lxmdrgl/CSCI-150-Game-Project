using System;
using UnityEngine;

using Game.Weapons;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace Game.CoreSystem
{
    public class WeaponInventory : CoreComponent
    {
        public event Action<int, WeaponData> OnWeaponDataChanged;

        [field: SerializeField] public WeaponDataSet weaponDataSet { get; private set; }
        [field: SerializeField] public WeaponData[] currentWeaponData { get; private set; }
        [field: SerializeField] public List<WeaponData> oldWeaponData { get; private set; }


        public bool TrySetWeapon(WeaponData newData, int index)
        {
            WeaponData oldData;

            if (index >= currentWeaponData.Length)
            {
                oldData = null;
                return false;
            }

            oldData = currentWeaponData[index];
            currentWeaponData[index] = newData;

            if (oldData != null) {
                oldWeaponData.Add(oldData);
            }

            OnWeaponDataChanged?.Invoke(index, newData);

            return true;
        }

        public bool TryGetWeapon(int index, out WeaponData data)
        {
            if (index >= currentWeaponData.Length)
            {
                data = null;
                return false;
            }

            data = currentWeaponData[index];
            return true;
        }

        /* public bool TryGetEmptyIndex(out int index)
        {
            for (var i = 0; i < weaponData.Length; i++)
            {
                if (weaponData[i] is not null)
                    continue;

                index = i;
                return true;
            }

            index = -1;
            return false;
        }

        public WeaponSwapChoice[] GetWeaponSwapChoices()
        {
            var choices = new WeaponSwapChoice[weaponData.Length];

            for (var i = 0; i < weaponData.Length; i++)
            {
                var data = weaponData[i];

                choices[i] = new WeaponSwapChoice(data, i);
            }

            return choices;
        } */
    }

}