using System;
using System.Diagnostics;
using Game.Interaction;
using Game.Interaction.Interactables;
using Game.Weapons;
using UnityEngine;

namespace Game.CoreSystem
{
    public class StatsChange : CoreComponent
    {
        // public event Action<WeaponSwapChoiceRequest> OnChoiceRequested;
        // public event Action<WeaponData> OnWeaponDiscarded;

        private InteractableDetector interactableDetector;
        private Stats stats;
        private StatUpgradeData newStatsData;

        private MinorUpgrade minorUpgrade;

        private void HandleTryInteract(IInteractable interactable)
        {
            // UnityEngine.Debug.Log("HandleTryInteract called in StatsChange");

            if (interactable is not MinorUpgrade pickup)
            {
                // UnityEngine.Debug.Log("Interactable is not a MinorUpgrade");
                return;
            }

            // UnityEngine.Debug.Log("MinorUpgrade detected");

            minorUpgrade = pickup;

            newStatsData = minorUpgrade.GetContext();
            stats.UpdateStats(newStatsData.Health, newStatsData.Attack);

            minorUpgrade.Interact();

            newStatsData = null;
        }

        protected override void Awake()
        {
            base.Awake();

            interactableDetector = core.GetCoreComponent<InteractableDetector>();
            stats = core.GetCoreComponent<Stats>();
        }

        private void OnEnable()
        {
            // UnityEngine.Debug.Log("StatsChange OnEnable");
            interactableDetector.OnTryInteract += HandleTryInteract;
        }


        private void OnDisable()
        {
            interactableDetector.OnTryInteract -= HandleTryInteract;
        }
    }
}