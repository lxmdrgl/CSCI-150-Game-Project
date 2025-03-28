using System;
using UnityEngine;

using Game.Combat.Damage;
using UnityEngine.InputSystem;
// using Game.ModifierSystem;

namespace Game.CoreSystem
{
    public class DamageReceiver : CoreComponent, IDamageable
    {
        // [SerializeField] private GameObject damageParticles;

        /*
         * Modifiers allows us to perform some custom logic on our DamageData before we apply it here. An example where this is being used is by the Block weapon component.
         * Blocking works by assigning a modifier during the active block window of the shield that reduces the amount of damage the player will take. For example: If a shield
         * has a damage absorption property of 0.75 and we deal 10 damage, only 2.5 will actually end up getting removed from player stats after applying the modifier.
         */

        // public Modifiers<Modifier<DamageData>, DamageData> Modifiers { get; } = new();

        private Stats stats;
        private Death death;
        // private ParticleManager particleManager;
        public bool CanTakeDamage { get; set; }
        public Action OnIgnoreDamage;

        public void SetCanTakeDamage(bool value) 
        {
            CanTakeDamage = value;
        }

        public void Damage(DamageData data)
        {
            /* print($"Damage Amount Before Modifiers: {data.Amount}");

            // We must apply the modifiers before we do anything else with data. If there are no modifiers currently active, data will remain the same
            
            data = Modifiers.ApplyAllModifiers(data);

            print($"Damage Amount After Modifiers: {data.Amount}"); */

            if (data.Amount <= 0f)
            {
                return;
            }
            // Debug.Log($"called Ignore damage, {CanTakeDamage}");

            if (CanTakeDamage) {
                stats.Health.Decrease(data.Amount);
                PlayerInput input = data.Source.GetComponentInChildren<PlayerInput>();
                Debug.Log($"Deal {data.Amount} damage, Source: {input.playerIndex}, {CanTakeDamage}");

                if (input != null)
                {
                    if (input.playerIndex == 0)
                    {
                        PlayerPrefs.SetInt("player1Damage", PlayerPrefs.GetInt("player1Damage") + Mathf.RoundToInt(stats.Health.damageTaken));
                        death.Source = 0;
                    }
                    else
                    {
                        PlayerPrefs.SetInt("player2Damage", PlayerPrefs.GetInt("player2Damage") + Mathf.RoundToInt(stats.Health.damageTaken));
                        death.Source = 1;
                    }
                }

            } else {
                Debug.Log($"Ignore damage, {CanTakeDamage}");
                OnIgnoreDamage?.Invoke();
            }

            // if (data.Source)


            // particleManager.StartWithRandomRotation(damageParticles);
        }

        protected override void Awake()
        {
            base.Awake();

            stats = core.GetCoreComponent<Stats>();
            death = core.GetCoreComponent<Death>();
            CanTakeDamage = true;
            // particleManager = core.GetCoreComponent<ParticleManager>();
        }
    }
}