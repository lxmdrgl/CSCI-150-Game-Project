using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Game.Weapons.Components;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Basic Weapon Data", order = 0)]
    public class WeaponData : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public RuntimeAnimatorController AnimatorController { get; private set; }
        [field: SerializeField] public CombatInputs weaponIndex { get; private set; }
        [field: SerializeField] public int NumberOfAttacks { get; private set; }
        [field: SerializeField] public float AttackCounterCooldown { get; private set; }
        [field: SerializeField] public float AttackCooldown { get; private set; }

        [field: SerializeField] public WeaponData prereqWeapon;

        [field: SerializeReference] public List<ComponentData> ComponentData { get; private set; }

        public T GetData<T>()
        {
            return ComponentData.OfType<T>().FirstOrDefault();
        }

        public List<Type> GetAllDependencies()
        {
            return ComponentData.Select(component => component.ComponentDependency).ToList();
        }

        public void AddData(ComponentData data)
        {
            if(ComponentData.FirstOrDefault(t => t.GetType() == data.GetType()) != null) 
                return;
            
            ComponentData.Add(data);
        }
    }
}