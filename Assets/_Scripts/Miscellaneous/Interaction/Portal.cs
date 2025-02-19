using System;
using UnityEngine;
using UnityEngine.Serialization;

using Game.Weapons;

namespace Game.Interaction.Interactables
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Portal : MonoBehaviour, IInteractable<string>
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        [SerializeField] private SpriteRenderer spriteIcon;

        [SerializeField] private string location;

        //[SerializeField] private Location location;
        
        public string GetContext() => location;
        public void SetContext(string context)
        {
            location = context;
        }        

        public void Interact()
        {
            Destroy(gameObject);
        }

        public void EnableInteraction()
        {
            // bobber.StartBobbing();
        }

        public void DisableInteraction()
        {
            // bobber.StopBobbing();
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private void Awake()
        {
            Rigidbody2D ??= GetComponent<Rigidbody2D>();
            spriteIcon ??= GetComponentInChildren<SpriteRenderer>();
        }
    }
}