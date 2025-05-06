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

        AudioSource audioSource;

        [SerializeField] private List<AudioClip> footstepSounds;
        [SerializeField] private AudioClip jumpSound;

        private void HandleFootstep()
        {
             if (footstepSounds.Count == 0)
            {
                // test Debug.LogWarning("No footstep sounds assigned.");
                
            }
            else if (footstepSounds.Count > 0)
            {
                audioSource.clip = footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Count)];
                audioSource.Play();
             } 
            // test Debug.Log("Footstep sound played.");
            // test Debug.Log("audioPlayer: " + audioSource);
        }
        private void HandleJump()
        {
            if (jumpSound == null)
            {
                // test Debug.LogWarning("No jump sound assigned.");
                return;
            }
            // test Debug.Log("Jump sound played.");
        }

        protected override void Awake()
        {
            AnimationEventHandler = GetComponentInParent<SoundAnimationEventHandler>();
            audioSource = GetComponent<AudioSource>();
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
