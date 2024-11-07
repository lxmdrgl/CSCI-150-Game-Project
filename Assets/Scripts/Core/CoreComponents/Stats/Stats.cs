using System;
using UnityEngine;
using UnityEngine.UI;

using Game.CoreSystem.StatsSystem;

namespace Game.CoreSystem
{
    public class Stats : CoreComponent
    {
       [field: SerializeField] public Stat Health { get; private set; }
       [field: SerializeField] public Stat Poise { get; private set; }

       [SerializeField] private float poiseRecoveryRate;

       public Image filledBar;
       public Gradient gradient;
        
        protected override void Awake()
        {
            base.Awake();
            
            Health.Init();


            Poise.Init();
        }

        private void Update()
        {
            if (Poise.CurrentValue.Equals(Poise.MaxValue))
                return;
            
            Poise.Increase(poiseRecoveryRate * Time.deltaTime);

            filledBar.color = gradient.Evaluate(filledBar.fillAmount);
        }
    }
}
