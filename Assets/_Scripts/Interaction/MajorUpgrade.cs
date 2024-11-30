using System;
using UnityEngine;
using UnityEngine.Serialization;

using Game.Weapons;

namespace Game.Interaction.Interactables
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MajorUpgrade : MonoBehaviour, IInteractable<WeaponData>
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        [SerializeField] private SpriteRenderer spriteIcon;
        // [SerializeField] private Bobber bobber;
        
        [SerializeField] private WeaponData weaponData;
        [SerializeField] public CombatInputs weaponIndex;
        
        public WeaponData GetContext() => weaponData;
        public void SetContext(WeaponData context)
        {
            weaponData = context;

            // spriteIcon.sprite = weaponData.Icon;
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
            
            if(weaponData is null)
                return;

            // spriteIcon.sprite = weaponData.Icon;
        }
    }
}