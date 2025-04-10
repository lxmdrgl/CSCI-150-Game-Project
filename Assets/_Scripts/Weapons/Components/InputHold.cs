using Game.CoreSystem;
using UnityEngine;

namespace Game.Weapons.Components
{
    public class InputHold : WeaponComponent
    {
        private Animator anim;
        private KnockBackReceiver knockbackReceiver;

        private bool input;

        private bool minHoldPassed;

        protected override void HandleEnter()
        {
            base.HandleEnter();

            minHoldPassed = false;
            anim.SetBool("hold", true);
        }

        protected override void HandleExit()
        {
            base.HandleExit();
            anim.SetBool("hold", false);
        }

        private void HandleKnockbackActive()
        {
            anim.SetBool("break", true);
        }

        private void HandleCurrentInputChange(bool newInput)
        {
            input = newInput;

            SetAnimatorParameter();
        }

        private void HandleMinHoldPassed()
        {
            minHoldPassed = true;

            SetAnimatorParameter();
        }

        private void SetAnimatorParameter()
        {
            if (input)
            {
                anim.SetBool("hold", input);
                return;
            }

            if (minHoldPassed)
            {
                anim.SetBool("hold", false);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            anim = GetComponentInChildren<Animator>();
            knockbackReceiver = Core.GetCoreComponent<KnockBackReceiver>();

            weapon.OnCurrentInputChange += HandleCurrentInputChange;
            AnimationEventHandler.OnMinHoldPassed += HandleMinHoldPassed;
            knockbackReceiver.OnKnockBackActive += HandleKnockbackActive;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            weapon.OnCurrentInputChange -= HandleCurrentInputChange;
            AnimationEventHandler.OnMinHoldPassed -= HandleMinHoldPassed;
        }
    }
}