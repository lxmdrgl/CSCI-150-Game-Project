using System;
using UnityEngine;

namespace Game.CoreSystem.StatsSystem
{
    [Serializable]
    public class Stat
    {
        public event Action OnCurrentValueZero;
        public event Action OnValueChange;

        // public event Action<float> OnValueChangeFloat;
        
        [field: SerializeField] public float MaxValue { get; set; }

        public float damageTaken;

        public float CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = Mathf.Clamp(value, 0f, MaxValue);

                if (currentValue <= 0f)
                {
                    OnCurrentValueZero?.Invoke();
                }
            }
        }
        
        private float currentValue;

        public void Init()=>currentValue = MaxValue;
    

        public void Increase(float amount) {
            CurrentValue += amount;
            OnValueChange?.Invoke();
        }

        public void Decrease(float amount) {
            damageTaken = Mathf.Min(amount, CurrentValue);
            CurrentValue -= amount;    
            OnValueChange?.Invoke();
            // OnValueChangeFloat?.Invoke(amount);
        }

        public void IncreaseMaxValue(float amount) {
            float ratio = currentValue / MaxValue;
            MaxValue = Mathf.Floor(MaxValue * (1 + (amount/100)));
            currentValue = Mathf.Floor(ratio * MaxValue);
            Debug.Log("Health: " + MaxValue + " " + currentValue);
            OnValueChange?.Invoke();
        }
    }
}