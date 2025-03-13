using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

namespace Game.Weapons.Components
{
    public class Fall : WeaponComponent<FallData, AttackFall>
    {
        private Animator anim;
        private CoreSystem.Movement coreMovement;
        private CoreSystem.CollisionSenses coreCollisionSenses;
        private float velocity;
        private Vector2 angle;
        private bool isGrounded;

        private void HandleStartFall()
        {
            velocity = currentAttackData.Velocity;
            angle = currentAttackData.Angle;
            
            coreMovement.SetGravityScale(0f);
            coreMovement.SetVelocity(velocity, angle, coreMovement.FacingDirection);
        }

        private void HandleStopFall()
        {
            velocity = 0f;
            angle = Vector2.zero;

            coreMovement.SetGravityScale(5f);
            coreMovement.SetVelocity(velocity, angle, coreMovement.FacingDirection);
        }

        protected override void Start()
        {
            base.Start();

            coreMovement = Core.GetCoreComponent<CoreSystem.Movement>();
            coreCollisionSenses = Core.GetCoreComponent<CoreSystem.CollisionSenses>();
        }

        protected override void HandleEnter()
        {
            base.HandleEnter();

            anim.SetBool("hold", true);
        }

        protected override void Awake()
        {
            base.Awake();

            anim = GetComponentInChildren<Animator>();

            AnimationEventHandler.OnStartFall += HandleStartFall;
        }

        protected void Update()
        {
            if (!isAttackActive)
                return;

            isGrounded = coreCollisionSenses.Ground;

            if (isGrounded)
            {
                anim.SetBool("hold", false);
                HandleStopFall();
            }
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            AnimationEventHandler.OnStartFall -= HandleStartFall;
        }
    }
}