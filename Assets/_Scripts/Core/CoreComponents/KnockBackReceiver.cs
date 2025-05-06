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
            // test Debug.Log($"knock active back set to, {CanTakeKnockBack}");
        }

        public void KnockBack(KnockBackData data)
        {
            // data = Modifiers.ApplyAllModifiers(data);
            /* movement.SetVelocity(data.Strength, data.Angle, data.Direction);
            movement.CanSetVelocity = false;
            isKnockBackActive = true;
            knockBackStartTime = Time.time; */
            // test Debug.Log("Knock active: " + CanTakeKnockBack);

            if (CanTakeKnockBack) {
                movement.SetVelocity(data.Strength, data.Angle, data.Direction);
                movement.CanSetVelocity = false;
                isKnockBackActive = true;
                knockBackStartTime = Time.time;
                // // test Debug.Log("Knock active");
                // test Debug.Log($"take knockback, {CanTakeKnockBack}");
                OnKnockBackActive?.Invoke();
            } else {
                // test Debug.Log($"Ignore knockback false, {CanTakeKnockBack}");
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
                // // test Debug.Log("Knock inactive: " + (Time.time - knockBackStartTime));
                OnKnockBackInactive?.Invoke();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            movement = core.GetCoreComponent<Movement>();
            collisionSenses = core.GetCoreComponent<CollisionSenses>();
            CanTakeKnockBack = true;
        }
    }
}