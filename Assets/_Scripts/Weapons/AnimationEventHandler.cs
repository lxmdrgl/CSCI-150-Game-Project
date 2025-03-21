using System;
using UnityEngine;

namespace Game.Weapons
{
    public class AnimationEventHandler : MonoBehaviour
    {
        public event Action OnFinish;
        public event Action OnStartMovement;
        public event Action OnStopMovement;
        public event Action OnAttackAction;
        public event Action OnProjectileAction;
        public event Action<bool> OnAttackActionSetActive;
        public event Action<bool> OnParryActionSetActive;
        public event Action<bool> OnFlipSetActive; 
        public event Action<bool> OnInterruptableSetActive;
        public event Action<AttackPhases> OnEnterAttackPhase;
        public event Action OnUseInput;
        public event Action OnMinHoldPassed;
        public event Action OnStartFall;

        /*
         * This trigger is used to indicate in the weapon animation when the input should be "used" meaning the player has to release the input key and press it down again to trigger the next attack.
         * Generally this animation event is added to the first "action" frame of an animation. e.g the first sword strike frame, or the frame where the bow is released.
         */

        /* public event Action OnEnableInterrupt; 
        public event Action<bool> OnSetOptionalSpriteActive; */
        /*
         * Animations events used to indicate when a specific time window starts and stops in an animation. These windows are identified using the
         * AnimationWindows enum. These windows include things like when the shield's block is active and when it can parry.
         */
        /* public event Action<AnimationWindows> OnStartAnimationWindow;
        public event Action<AnimationWindows> OnStopAnimationWindow;

        public event Action<bool> OnGroundedSetActive; */

        private void AnimationFinishedTrigger() => OnFinish?.Invoke();
        private void StartMovementTrigger() => OnStartMovement?.Invoke();
        private void StopMovementTrigger() => OnStopMovement?.Invoke();
        // private void AttackActionTrigger() => OnAttackAction?.Invoke();
        private void ProjectileActionTrigger() => OnProjectileAction?.Invoke();
        private void StartAttackActionTrigger() => OnAttackActionSetActive?.Invoke(true);
        private void StopAttackActionTrigger() => OnAttackActionSetActive?.Invoke(false);
        private void StartParryActionTrigger() => OnParryActionSetActive?.Invoke(true);
        private void StopParryActionTrigger() => OnParryActionSetActive?.Invoke(false);
        private void MinHoldPassedTrigger() => OnMinHoldPassed?.Invoke();
        private void UseInputTrigger() => OnUseInput?.Invoke();
        private void SetFlipActive() => OnFlipSetActive?.Invoke(true);
        private void SetFlipInactive() => OnFlipSetActive?.Invoke(false);
        private void SetInterruptableSetActive() => OnInterruptableSetActive?.Invoke(true);
        private void SetInterruptableSetInActive() => OnInterruptableSetActive?.Invoke(false);
        private void EnterAttackPhase(AttackPhases phase) => OnEnterAttackPhase?.Invoke(phase);
        private void StartFallTrigger() => OnStartFall?.Invoke();


        /* private void SetOptionalSpriteEnabled() => OnSetOptionalSpriteActive?.Invoke(true);
        private void SetOptionalSpriteDisabled() => OnSetOptionalSpriteActive?.Invoke(false);

        private void StartAnimationWindow(AnimationWindows window) => OnStartAnimationWindow?.Invoke(window);
        private void StopAnimationWindow(AnimationWindows window) => OnStopAnimationWindow?.Invoke(window);

        private void EnableInterrupt() => OnEnableInterrupt?.Invoke();
 */
        
        public void OnAttackActionInvoke() => OnAttackAction?.Invoke();
        public void OnAttackActionSetActiveInvoke(bool value) => OnAttackActionSetActive?.Invoke(value);
    }
}