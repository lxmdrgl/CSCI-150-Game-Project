using System;
using UnityEngine;

using Game.Weapons;

namespace Game.CoreSystem
{
    public class WeaponInventory : CoreComponent
    {
        public event Action<int, WeaponData> OnWeaponDataChanged;

        [field: SerializeField] public WeaponData[] weaponData { get; private set; }

        public bool TrySetWeapon(WeaponData newData, int index, out WeaponData oldData)
        {
            if (index >= weaponData.Length)
            {
                oldData = null;
                return false;
            }

            oldData = weaponData[index];
            weaponData[index] = newData;

            OnWeaponDataChanged?.Invoke(index, newData);

            return true;
        }

        public bool TryGetWeapon(int index, out WeaponData data)
        {
            if (index >= weaponData.Length)
            {
                data = null;
                return false;
            }

            data = weaponData[index];
            return true;
        }

        public bool TryGetEmptyIndex(out int index)
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
        }
    }
}