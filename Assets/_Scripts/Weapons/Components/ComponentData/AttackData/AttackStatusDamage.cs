using System;
using UnityEngine;

namespace Game.Weapons.Components
{
    [Serializable]
    public class AttackStatusDamage : AttackData
    {
        [field: SerializeField] public float Amount { get; private set; }
        [field: SerializeField] public float Damage { get; protected set;}
        [field: SerializeField] public float Stun { get; protected set;}
        
    }
    [Serializable]
    public class FireStatusDamage : AttackStatusDamage
    {
        [field: SerializeField] public int Ticks { get; private set;}
        [field: SerializeField] public float Delay { get; private set;}
        [field: SerializeField] public float Mult { get; private set;}
    }
}