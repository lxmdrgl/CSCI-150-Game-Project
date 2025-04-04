using System;
using UnityEngine;
using UnityEngine.Serialization;

using Game.Weapons;
using System.Collections;

namespace Game.Interaction.Interactables
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MinorUpgrade : MonoBehaviour, IInteractable<StatUpgradeDataSet>
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        [SerializeField] private SpriteRenderer spriteIcon;
        private Vector3 basePosition; // The position to bounce around
        private bool isBouncing = false;
        [SerializeField] private StatUpgradeDataSet statUpgradeDataSet;

        public StatUpgradeDataSet GetContext() => statUpgradeDataSet;
        public void SetContext(StatUpgradeDataSet context)
        {
            statUpgradeDataSet = context;
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

        private void Start()
        {
            // Set the base position as the final position after pop-out (set by Chest)
            basePosition = transform.position;
            StartCoroutine(ContinuousBounce());
        }

        private IEnumerator ContinuousBounce()
        {
            isBouncing = true;
            float bounceHeight = 0.2f; // Adjust height of bounce
            float bounceSpeed = 2f;    // Adjust speed of bounce
            float time = 0f;

            while (isBouncing) // Runs until the object is destroyed or stopped
            {
                time += Time.deltaTime * bounceSpeed;
                float height = Mathf.Abs(Mathf.Sin(time)) * bounceHeight; // Smooth up-down motion
                transform.position = basePosition + Vector3.up * height;
                yield return null;
            }
        }

        private void OnDestroy()
        {
            isBouncing = false; // Ensures the coroutine stops when the object is destroyed
        }
    }
    
}