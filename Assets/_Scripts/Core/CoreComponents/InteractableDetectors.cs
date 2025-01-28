using System;
using System.Collections.Generic;
using UnityEngine;

using Game.Interaction;
using Game.Utilities;

namespace Game.CoreSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableDetector : CoreComponent
    {
        public Action<IInteractable> OnTryInteract;

        private readonly List<IInteractable> interactables = new();

        private IInteractable closestInteractable;

        private float distanceToClosestInteractable = float.PositiveInfinity;

        [ContextMenu("TryInteract")]
        public void TryInteract(bool inputValue)
        {
            if(!inputValue || closestInteractable is null)
                return;

            Debug.Log("TryInteract called with closest interactable: " + closestInteractable);
            OnTryInteract?.Invoke(closestInteractable);
        }
        
        private void Update()
        {
            if (interactables.Count <= 0)
                return;

            distanceToClosestInteractable = float.PositiveInfinity;
            var oldClosestInteractable = closestInteractable;

            if (closestInteractable is not null)
            {
                distanceToClosestInteractable = FindDistanceTo(closestInteractable);
            }

            foreach (var interactable in interactables)
            {
                if (interactable == closestInteractable)
                    continue;

                if (FindDistanceTo(interactable) >= distanceToClosestInteractable)
                    continue;

                closestInteractable = interactable;
                distanceToClosestInteractable = FindDistanceTo(closestInteractable);
            }

            if (closestInteractable == oldClosestInteractable)
                return;

            oldClosestInteractable?.DisableInteraction();
            closestInteractable?.EnableInteraction();
        }

        private float FindDistanceTo(IInteractable interactable)
        {
            return Vector3.Distance(transform.position, interactable.GetPosition());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.IsInteractable(out var interactable))
            {
                // Debug.Log("Interactable entered: " + interactable);
                interactables.Add(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.IsInteractable(out var interactable))
            {
                interactables.Remove(interactable);

                if (interactable == closestInteractable)
                {
                    interactable.DisableInteraction();
                    closestInteractable = null;
                }
            }
        }
    }
}