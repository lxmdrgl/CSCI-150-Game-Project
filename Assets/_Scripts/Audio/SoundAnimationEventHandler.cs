using System;
using UnityEngine;

namespace Game.Weapons
{
    public class SoundAnimationEventHandler : MonoBehaviour
    {
        public event Action OnFootstep;

        public event Action OnJump;

        private void FootstepTrigger() => OnFootstep?.Invoke();
        private void JumpTrigger() => OnJump?.Invoke();
    }
}