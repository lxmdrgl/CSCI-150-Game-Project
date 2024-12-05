using System;
using UnityEngine;
using UnityEngine.Serialization;

using Game.Combat.KnockBack;
// using Game.ModifierSystem;

namespace Game.CoreSystem
{
    public class KnockBackReceiver : CoreComponent, IKnockBackable
    {
        // public Modifiers<Modifier<KnockBackData>, KnockBackData> Modifiers { get; } = new();

        [SerializeField] private float maxKnockBackTime = 0.2f;

        public event Action OnKnockBackActive;
        public event Action OnKnockBackInactive;

        private bool isKnockBackActive;
        private float knockBackStartTime;

        private Movement movement;
        private CollisionSenses collisionSenses;

        public bool CanTakeKnockBack { get; set; }

        public override void LogicUpdate()
        {
            CheckKnockBack();
        }

        public void SetCanTakeKnockBack(bool value) 
        {
            CanTakeKnockBack = value;
        }

        public void KnockBack(KnockBackData data)
        {
            // data = Modifiers.ApplyAllModifiers(data);
            
            movement.SetVelocity(data.Strength, data.Angle, data.Direction);
            movement.CanSetVelocity = false;
            isKnockBackActive = true;
            knockBackStartTime = Time.time;
            // Debug.Log("Knock active");
            OnKnockBackActive?.Invoke();
            if (CanTakeKnockBack) {
                movement.SetVelocity(data.Strength, data.Angle, data.Direction);
                movement.CanSetVelocity = false;
                isKnockBackActive = true;
                knockBackStartTime = Time.time;
                // Debug.Log("Knock active");
                OnKnockBackActive?.Invoke();
                Debug.Log($"take knockback, {CanTakeKnockBack}");
            } else {
                Debug.Log($"Ignore knockback, {CanTakeKnockBack}");
            }
        }

        private void CheckKnockBack()
        {
            if (isKnockBackActive
                && ((movement.CurrentVelocity.y <= 0.01f && collisionSenses.Ground)
                    || Time.time >= knockBackStartTime + maxKnockBackTime)
               )
            {
                isKnockBackActive = false;
                movement.CanSetVelocity = true;
                // Debug.Log("Knock inactive: " + (Time.time - knockBackStartTime));
                OnKnockBackInactive?.Invoke();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            movement = core.GetCoreComponent<Movement>();
            collisionSenses = core.GetCoreComponent<CollisionSenses>();
        }
    }
}