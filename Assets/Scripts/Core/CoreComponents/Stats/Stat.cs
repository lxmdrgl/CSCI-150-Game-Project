using System;
using UnityEngine;

namespace Game.CoreSystem.StatsSystem
{
    [Serializable]
    public class Stat
    {
        public event Action OnCurrentValueZero;
        public event Action OnValueChange;
        
        [field: SerializeField] public float MaxValue { get; set; }

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

        public void Init() => CurrentValue = MaxValue;

        public void Increase(float amount) {
            CurrentValue += amount;
            OnValueChange?.Invoke();
        }

        public void Decrease(float amount) {
            CurrentValue -= amount;    
            OnValueChange?.Invoke();
        }

        public void setHP(float maxVal, float currVal)
        {
            MaxValue = maxVal;
            currentValue = currVal;
        }

        public float getCurrentVal()
        {
            return currentValue;
        }
        public float getMaxVal()
        {
            return MaxValue;
        }
    }
}