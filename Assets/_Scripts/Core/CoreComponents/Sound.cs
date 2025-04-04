using System;
using System.Collections;
using System.Collections.Generic;
using Game.Weapons;
using UnityEngine;

namespace Game.CoreSystem
{
    public class Sound : CoreComponent
    {
        SoundAnimationEventHandler AnimationEventHandler;

        AudioPlayer audioPlayer;

        [SerializeField] private List<AudioClip> footstepSounds;
        [SerializeField] private AudioClip jumpSound;

        private void HandleFootstep()
        {
            /* if (footstepSounds.Count == 0)
            {
                Debug.LogWarning("No footstep sounds assigned.");
                return;
            } */
            Debug.Log("Footstep sound played.");
            Debug.Log("audioPlayer: " + audioPlayer);
        }
        private void HandleJump()
        {
            if (jumpSound == null)
            {
                Debug.LogWarning("No jump sound assigned.");
                return;
            }
            Debug.Log("Jump sound played.");
        }

        protected override void Awake()
        {
            AnimationEventHandler = GetComponentInParent<SoundAnimationEventHandler>();
            audioPlayer = GameObject.FindGameObjectsWithTag("Audio")[0].GetComponent<AudioPlayer>();
        }
        protected void Start()
        {
            
            AnimationEventHandler.OnFootstep += HandleFootstep;
            AnimationEventHandler.OnJump += HandleJump;
        }

        protected void OnDestroy()
        {
            
            AnimationEventHandler.OnFootstep -= HandleFootstep;
            AnimationEventHandler.OnJump -= HandleJump;
        }
    }
}
