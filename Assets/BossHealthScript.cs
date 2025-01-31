using Game.CoreSystem;
using Game.CoreSystem.StatsSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarBoss : MonoBehaviour
{
    public Slider healthSliderBoss;
    public Slider easeSliderBoss;
    public TMP_Text healthTextBoss;
    public Stats stats;
    public float lerpSpeed = 0.05f;

    void Start() {
        healthSliderBoss.maxValue = stats.Health.MaxValue;
        healthSliderBoss.value = stats.Health.CurrentValue;
        healthTextBoss.text = healthSliderBoss.value + "/" + healthSliderBoss.maxValue;

        easeSliderBoss.maxValue = stats.Health.MaxValue;
        easeSliderBoss.value = stats.Health.CurrentValue;
    }

    void FixedUpdate() {
        if (healthSliderBoss.value != easeSliderBoss.value) {
            easeSliderBoss.value = Mathf.Lerp(easeSliderBoss.value, healthSliderBoss.value, lerpSpeed);
        }
    }

    private void UpdateSlider() {
        healthSliderBoss.maxValue = stats.Health.MaxValue;
        healthSliderBoss.value = stats.Health.CurrentValue;
        easeSliderBoss.maxValue = stats.Health.MaxValue;
        healthTextBoss.text = healthSliderBoss.value + "/" + healthSliderBoss.maxValue;
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

