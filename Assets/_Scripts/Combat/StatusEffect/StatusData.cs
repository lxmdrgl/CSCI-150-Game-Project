using UnityEngine;
using Game.CoreSystem;
using System;

namespace Game.Combat.Status
{
    public abstract class StatusData
    {
        public float Amount { get; protected set;}
        public float Damage { get; protected set;}
        public float Stun { get; protected set;}
        public GameObject Source { get; protected set;}
        public Stats Stats { get; protected set;}
        public DamageReceiver DamageReceiver { get; protected set;}
        public StunDamageReceiver StunDamageReceiver { get; protected set;}

        public StatusData(float amount, float damage, float stun, GameObject source)
        {
            Amount = amount;
            Damage = damage;
            Stun = stun;
            Source = source;
        }

        public virtual void ApplyStatus(Stats stats, DamageReceiver damageReceiver, StunDamageReceiver stunDamageReceiver, Action onCompleteCallback)
        {
            Stats = stats;
            DamageReceiver = damageReceiver;
            StunDamageReceiver = stunDamageReceiver;
        }
        public abstract void ReapplyStatus(StatusData stats);
    }
}