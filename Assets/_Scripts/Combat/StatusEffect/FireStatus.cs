using System;
using System.Collections;
using Game.Combat.Damage;
using Game.CoreSystem;
using UnityEngine;

namespace Game.Combat.Status
{
    public class FireStatus : StatusData
    {
        public int Ticks { get; private set;}
        public float Delay { get; private set;}
        public float Mult { get; private set;}
        public int Count { get; private set;}
        public int index { get; private set;}
        public event Action OnComplete;

        public FireStatus(float amount, float damage, float stun, GameObject source, int ticks, float delay, float mult) : base (amount, damage, stun, source)
        {
            Ticks = ticks;
            Delay = delay;
            Mult = mult;
        }

        public override void ApplyStatus(Stats stats, DamageReceiver damageReceiver, StunDamageReceiver stunDamageReceiver, Action onCompleteCallback)
        {
            base.ApplyStatus(stats, damageReceiver, stunDamageReceiver, onCompleteCallback);

            OnComplete = onCompleteCallback;

            float newMult = 1f + (Count * Mult / 100f);
            DamageReceiver.Damage(new DamageData(Damage * newMult, Source));
            // Stats.Health.Decrease(Damage * newMult);
            Stats.Stun.Decrease(Stun * newMult);
            Count = 0;
            Debug.Log($"Apply Fire status first: {Damage}, {Stun}, {newMult}, {Count}");

            if (Stats.isActiveAndEnabled)
            {
                Stats.GetComponent<MonoBehaviour>().StartCoroutine(ApplyStatusOverTime());
            }
        }

        private IEnumerator ApplyStatusOverTime()
        {
            for (index = 1; index < Ticks; index++)
            {
                yield return new WaitForSeconds(Delay);

                float newMult = 1f + (Count * Mult / 100f);
                DamageReceiver.Damage(new DamageData(Damage * newMult, Source));
                // Stats.Health.Decrease(Damage * newMult);
                Stats.Stun.Decrease(Stun * newMult);
                Debug.Log($"Apply Fire status time: {Damage}, {Stun}, {newMult}, {Count}, t{index}");
            }
            
            OnComplete?.Invoke();
        }

        public override void ReapplyStatus(StatusData data)
        {
            if (data is FireStatus fireData)
            {
                Damage = fireData.Damage;
                Stun = fireData.Stun;
                // Ticks += fireData.Ticks;
                Ticks = fireData.Ticks;
                index = 0;
                Delay = fireData.Delay;
                Count++;
                // if (fireData.Damage > Damage)
                // {
                // }
                // if (fireData.Stun > Stun)
                // {
                // }
                // if (fireData.Delay < Delay)
                // {
                // }
                Debug.Log($"Reapply status Fire: {Damage}, {Stun}, {Ticks}, {Delay}, {Count}");
            }

        }
    }
}