using System;
using UnityEngine;
using UnityEngine.Serialization;

using Game.Weapons;

namespace Game.Interaction.Interactables
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MajorUpgrade : MonoBehaviour, IInteractable<WeaponDataSet>
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        [SerializeField] private SpriteRenderer spriteIcon;
        // [SerializeField] private Bobber bobber;
        
        [SerializeField] private WeaponDataSet weaponDataSet;
        
        public WeaponDataSet GetContext() => weaponDataSet;
        public void SetContext(WeaponDataSet context)
        {
            weaponDataSet = context;

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
            
            if(weaponDataSet is null)
                return;

            // spriteIcon.sprite = weaponData.Icon;
        }
    }
}