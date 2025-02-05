using System;
using UnityEngine;
using UnityEngine.Serialization;

using Game.Weapons;

namespace Game.Interaction.Interactables
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MinorUpgrade : MonoBehaviour, IInteractable<StatUpgradeData>
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        [SerializeField] private SpriteRenderer spriteIcon;

        [SerializeField] private StatUpgradeData statUpgradeData;

        public StatUpgradeData GetContext() => statUpgradeData;
        public void SetContext(StatUpgradeData context)
        {
            statUpgradeData = context;
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