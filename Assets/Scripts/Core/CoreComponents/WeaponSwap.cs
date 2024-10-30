using System;
using Game.Interaction;
using Game.Interaction.Interactables;
using Game.Weapons;

namespace Game.CoreSystem
{
    public class WeaponSwap : CoreComponent
    {
        public event Action<WeaponSwapChoiceRequest> OnChoiceRequested;
        public event Action<WeaponData> OnWeaponDiscarded;

        private InteractableDetector interactableDetector;
        private WeaponInventory weaponInventory;

        private WeaponData newWeaponData;

        private MajorUpgrade majorUpgrade;

        private void HandleTryInteract(IInteractable interactable)
        {
            if (interactable is not MajorUpgrade pickup)
                return;

            majorUpgrade = pickup;

            newWeaponData = majorUpgrade.GetContext();

            if (weaponInventory.TryGetEmptyIndex(out var index))
            {
                weaponInventory.TrySetWeapon(newWeaponData, index, out _);
                interactable.Interact();
                newWeaponData = null;
                return;
            }

            OnChoiceRequested?.Invoke(new WeaponSwapChoiceRequest(
                HandleWeaponSwapChoice,
                weaponInventory.GetWeaponSwapChoices(),
                newWeaponData
            ));
        }

        private void HandleWeaponSwapChoice(WeaponSwapChoice choice)
        {
            if (!weaponInventory.TrySetWeapon(newWeaponData, choice.Index, out var oldData)) 
                return;
            
            newWeaponData = null;

            OnWeaponDiscarded?.Invoke(oldData);
                
            if (majorUpgrade is null)
                return;

            majorUpgrade.Interact();
            
        }

        protected override void Awake()
        {
            base.Awake();

            interactableDetector = core.GetCoreComponent<InteractableDetector>();
            weaponInventory = core.GetCoreComponent<WeaponInventory>();
        }

        private void OnEnable()
        {
            interactableDetector.OnTryInteract += HandleTryInteract;
        }


        private void OnDisable()
        {
            interactableDetector.OnTryInteract -= HandleTryInteract;
        }
    }
}