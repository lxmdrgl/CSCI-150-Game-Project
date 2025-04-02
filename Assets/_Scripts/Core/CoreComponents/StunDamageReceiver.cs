using Game.Combat.StunDamage;
using UnityEngine;
using UnityEngine.InputSystem;
// using Game.ModifierSystem;

namespace Game.CoreSystem
{
    public class StunDamageReceiver : CoreComponent, IStunDamageable
    {
        private Stats stats;

        // public Modifiers<Modifier<PoiseDamageData>, PoiseDamageData> Modifiers { get; } = new();

        public void DamageStun(StunDamageData data)
        {
            // data = Modifiers.ApplyAllModifiers(data);
            
            stats.Stun.Decrease(data.Amount);

            PlayerInput input = data.Source.GetComponentInChildren<PlayerInput>();
            Debug.Log($"Deal {data.Amount} stun");

            if (input != null)
            {
                if (input.playerIndex == 0)
                {
                    PlayerPrefs.SetInt("player1Stun", PlayerPrefs.GetInt("player1Stun") + Mathf.RoundToInt(stats.Stun.damageTaken));
                }
                else
                {
                    PlayerPrefs.SetInt("player2Stun", PlayerPrefs.GetInt("player2Stun") + Mathf.RoundToInt(stats.Stun.damageTaken));
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();

            stats = core.GetCoreComponent<Stats>();
        }
    }
}