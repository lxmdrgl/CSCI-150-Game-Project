using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.CoreSystem;
using Game.CoreSystem.StatsSystem;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] public Stats stats;
    public float lerpSpeed = 0.05f;

    public Transform healthBarTransform;
    
  //  public void UpdateHealthBar(float currentValue, float maxValue)
 //   {
  //      slider.value = currentValue / maxValue;
  //  }

    void Awake() {
    }
    
   void Start() {
        slider.maxValue = stats.Health.MaxValue;
        slider.value = stats.Health.CurrentValue;
        
    }

    private void UpdateSlider() {
        slider.maxValue = stats.Health.MaxValue;
        slider.value = stats.Health.CurrentValue;
    }

    private void OnEnable()
    {
        stats.Health.OnValueChange += UpdateSlider;
    }

    private void OnDisable()
    {
        stats.Health.OnValueChange -= UpdateSlider;
    }
}
