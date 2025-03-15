using System;
using System.Linq;
using UnityEngine;

using Game.Weapons.Components;

namespace Game.Weapons.Components
{
    public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
    {
        private SpriteRenderer baseSpriteRenderer;
        private SpriteRenderer weaponSpriteRenderer;
        
        private int currentWeaponSpriteIndex;

        private Sprite[] currentPhaseSprites;

        protected override void HandleEnter()
        {
            base.HandleEnter();
            
            currentWeaponSpriteIndex = 0;
        }

        private void HandleEnterAttackPhase(AttackPhases phase)
        {    
            currentWeaponSpriteIndex = 0;

            currentPhaseSprites = currentAttackData.PhaseSprites.FirstOrDefault(data => data.Phase == phase).Sprites;

            // Debug.LogWarning($"HandleEnterAttackPhase, phase: {phase}");
            if (currentPhaseSprites == null || currentPhaseSprites.Length == 0)
            {
                Debug.LogError($"No sprites found for phase: {phase}");
            }
            else
            {
                // Debug.LogWarning($"Sprites found for phase: {phase}, count: {currentPhaseSprites.Length}");
            }
        }

        private void HandleBaseSpriteChange(SpriteRenderer sr)
        {
            if (!isAttackActive)
            {
                weaponSpriteRenderer.sprite = null;
                return;
            }
            // Debug.LogWarning($"currentWeaponSpriteIndex: {currentWeaponSpriteIndex}");
            // Debug.LogWarning($"currentPhaseSprites: {currentPhaseSprites.Length}");
            if (currentWeaponSpriteIndex >= currentPhaseSprites.Length)
            {
                Debug.LogWarning($"{weapon.name} weapon sprites length mismatch");
                return;
            }
            
            weaponSpriteRenderer.sprite = currentPhaseSprites[currentWeaponSpriteIndex];

            currentWeaponSpriteIndex++;
        }

        protected override void Start()
        {
            base.Start();

            baseSpriteRenderer = weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            weaponSpriteRenderer = weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();
            
            data = weapon.Data.GetData<WeaponSpriteData>();
            
            baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);

            AnimationEventHandler.OnEnterAttackPhase += HandleEnterAttackPhase;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            baseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
            
            AnimationEventHandler.OnEnterAttackPhase -= HandleEnterAttackPhase;
        }
    }
}