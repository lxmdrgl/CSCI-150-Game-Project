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
    [SerializeField] public Movement movement;
    public float lerpSpeed = 0.05f;

    public RectTransform healthBarTransform;

    void Awake() {
        healthBarTransform = GetComponent<RectTransform>();
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
        movement.OnFlip += UpdateFlip; 
    }

    private void UpdateFlip() {
        healthBarTransform?.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDisable()
    {
        stats.Health.OnValueChange -= UpdateSlider;
        movement.OnFlip -= UpdateFlip;
    }
}
