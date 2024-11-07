using Game.CoreSystem;
using Game.CoreSystem.StatsSystem;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public Stats stats;

    void Start() {
        healthSlider.maxValue = stats.Health.MaxValue;
        healthSlider.value = stats.Health.CurrentValue;
    }

    private void UpdateSlider() {
        healthSlider.maxValue = stats.Health.MaxValue;
        healthSlider.value = stats.Health.CurrentValue;
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
