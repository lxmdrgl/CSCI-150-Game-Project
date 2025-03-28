using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Game.CoreSystem;
using Game.Weapons.Components;

namespace Game.Weapons
{
    public class WeaponGenerator : MonoBehaviour
    {
        public event Action OnWeaponGenerating;
        
        [SerializeField] private Weapon weapon;
        [SerializeField] private CombatInputs combatInput;

        private List<WeaponComponent> componentAlreadyOnWeapon = new List<WeaponComponent>();

        private List<WeaponComponent> componentsAddedToWeapon = new List<WeaponComponent>();

        private List<Type> componentDependencies = new List<Type>();

        private Animator anim;

        private WeaponInventory weaponInventory;

        private void GenerateWeapon(WeaponData data)
        {
            // Debug.Log($"Generate Weapon, Data: {data}");

            OnWeaponGenerating?.Invoke();
            
            weapon.SetData(data);

            if (data is null)
            {
                weapon.SetCanEnterAttack(false);
                // Debug.LogError("WeaponData is null");
                return;
            }
            
            componentAlreadyOnWeapon.Clear();
            componentsAddedToWeapon.Clear();
            componentDependencies.Clear();

            componentAlreadyOnWeapon = GetComponents<WeaponComponent>().ToList();

            componentDependencies = data.GetAllDependencies();

            foreach (var dependency in componentDependencies)
            {
                if(componentsAddedToWeapon.FirstOrDefault(component => component.GetType() == dependency))
                    continue;

                var weaponComponent =
                    componentAlreadyOnWeapon.FirstOrDefault(component => component.GetType() == dependency);

                if (weaponComponent == null)
                {
                    weaponComponent = gameObject.AddComponent(dependency) as WeaponComponent;
                }
                
                weaponComponent.Init();
                
                componentsAddedToWeapon.Add(weaponComponent);
            }

            var componentsToRemove = componentAlreadyOnWeapon.Except(componentsAddedToWeapon);
            
            foreach (var weaponComponent in componentsToRemove)
            {
                Destroy(weaponComponent);
            }

            anim.runtimeAnimatorController = data.AnimatorController;
            
            weapon.SetCanEnterAttack(true);
            // Debug.Log("can enter attack");
        }
        
        private void HandleWeaponDataChanged(int inputIndex, WeaponData data)
        {
            if (inputIndex != (int)combatInput)
                return;

            if (data == null)
            {
                Debug.LogError($"WeaponData is null for inputIndex: {inputIndex}");
                return;
            }

            GenerateWeapon(data);
        }
        
        #region Plumbing

        private void Start()
        {
            // Debug.Log("Weapon: " + weapon);
            weaponInventory = weapon.Core.GetCoreComponent<WeaponInventory>();

            weaponInventory.OnWeaponDataChanged += HandleWeaponDataChanged;
            
            anim = GetComponentInChildren<Animator>();

            if (weaponInventory.TryGetWeapon((int)combatInput, out var data))
            {
                GenerateWeapon(data);
            }
        }

        private void OnDisable()
        {
            // weaponInventory.OnWeaponDataChanged -= HandleWeaponDataChanged;
        }

        #endregion
    }
}
