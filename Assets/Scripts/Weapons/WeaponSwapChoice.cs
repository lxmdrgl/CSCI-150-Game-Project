using System;

namespace Game.Weapons
{
    public class WeaponSwapChoiceRequest
    {
        public WeaponSwapChoice[] Choices { get; }
        public WeaponData NewWeaponData { get; }
        public Action<WeaponSwapChoice> Callback;

        public WeaponSwapChoiceRequest(
            Action<WeaponSwapChoice> callback,
            WeaponSwapChoice[] choices,
            WeaponData newWeaponData
        )
        {
            Callback = callback;
            Choices = choices;
            NewWeaponData = newWeaponData;
        }
    }

    public class WeaponSwapChoice
    {
        public WeaponData WeaponData { get; }
        public int Index { get; }

        public WeaponSwapChoice(WeaponData weaponData, int index)
        {
            WeaponData = weaponData;
            Index = index;
        }
    }
}