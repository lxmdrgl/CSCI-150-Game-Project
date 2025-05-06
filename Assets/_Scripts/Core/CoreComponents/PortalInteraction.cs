using System;
using Game.Interaction;
using Game.Interaction.Interactables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.CoreSystem
{
    public class PortalInteraction : CoreComponent
    {

        private InteractableDetector interactableDetector;

        private string locationData;

        private Portal portal;

        private string selectedProfileId = "";

        private void HandleTryInteract(IInteractable interactable)
        {
            // test UnityEngine.// Debug.Log("HandleTryInteract called in Portal");
            
            if (interactable is not Portal pickup)
                return;

            portal = pickup;

            locationData = portal.GetContext();
            

            PlayerPrefs.SetFloat("runTime", PlayerPrefs.GetFloat("runTime") + Time.timeSinceLevelLoad);
            SceneManager.LoadScene(locationData);

            portal.Interact();
            locationData = null;
        }


        protected override void Awake()
        {
            base.Awake();

            interactableDetector = core.GetCoreComponent<InteractableDetector>();
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