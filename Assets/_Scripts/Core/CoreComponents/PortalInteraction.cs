using System;
using Game.Interaction;
using Game.Interaction.Interactables;
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
            UnityEngine.Debug.Log("HandleTryInteract called in Portal");
            
            if (interactable is not Portal pickup)
                return;

            portal = pickup;

            locationData = portal.GetContext();
            
            if(DataPersistenceManager.instance.disableDataPersistence)
            {
                SceneManager.LoadScene(locationData);
                return;
            }
            else
            {
                DataPersistenceManager.instance.RestartGame( selectedProfileId, SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(locationData);

                DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);
                DataPersistenceManager.instance.SaveGame();
            }

            portal.Interact();
            locationData = null;

            
        }


        protected override void Awake()
        {
            base.Awake();

            interactableDetector = core.GetCoreComponent<InteractableDetector>();

            if(DataPersistenceManager.instance.disableDataPersistence == false)
            {
                selectedProfileId = DataPersistenceManager.instance.GetSelectedProfileId();
            }
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