using System;
using UnityEngine;

using Game.Weapons.Components;

namespace Game.Weapons.Components
{
    public class Movement : WeaponComponent<MovementData, AttackMovement>
    {
        private CoreSystem.Movement coreMovement;

        private float velocity;
        // private Vector2 direction;

        private void HandleStartMovement()
        {
            velocity = currentAttackData.VelocityX;
            // direction = currentAttackData.Direction;
            
            coreMovement.SetVelocityX(velocity * coreMovement.FacingDirection);
            coreMovement.SetVelocityY(0f);
        }

        private void HandleStopMovement()
        {
            velocity = 0f;
            // direction = Vector2.zero;

            coreMovement.SetVelocityX(velocity * coreMovement.FacingDirection);
        }

        protected override void HandleEnter()
        {
            base.HandleEnter();
            
            velocity = 0f;
            // direction = Vector2.zero;
        }

        private void FixedUpdate()
        {
            if(!isAttackActive)
                return;
            
            coreMovement.SetVelocityX(velocity * coreMovement.FacingDirection);
        }

        protected override void Start()
        {
            base.Start();

            coreMovement = Core.GetCoreComponent<CoreSystem.Movement>();
            
            AnimationEventHandler.OnStartMovement += HandleStartMovement;
            AnimationEventHandler.OnStopMovement += HandleStopMovement;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            AnimationEventHandler.OnStartMovement -= HandleStartMovement;
            AnimationEventHandler.OnStopMovement -= HandleStopMovement;
        }
    }
}