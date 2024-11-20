using System;
using UnityEngine;
using UnityEngine.UI;

using Game.CoreSystem.StatsSystem;

namespace Game.CoreSystem
{
    public class Stats : CoreComponent
    {
       [field: SerializeField] public Stat Health { get; private set; }
       [field: SerializeField] public Stat Stun { get; private set; }

       [SerializeField] private float stunRecoveryRate;

       public Image filledBar;
       public Gradient gradient;
        
        protected override void Awake()
        {
            base.Awake();
            
            Health.Init();
            Stun.Init();
        }

    }
}
