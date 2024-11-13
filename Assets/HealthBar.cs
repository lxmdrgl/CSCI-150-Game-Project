using Game.CoreSystem;
using Game.CoreSystem.StatsSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeSlider;
    public TMP_Text healthText;
    public Stats stats;
    public float lerpSpeed = 0.05f;

    void Start() {
        healthSlider.maxValue = stats.Health.MaxValue;
        healthSlider.value = stats.Health.CurrentValue;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;

        easeSlider.maxValue = stats.Health.MaxValue;
        easeSlider.value = stats.Health.CurrentValue;

        // Alex was here
    }

    void Update() {
        if (healthSlider.value != easeSlider.value) {
            easeSlider.value = Mathf.Lerp(easeSlider.value, healthSlider.value, lerpSpeed);
        }
    }

    private void UpdateSlider() {
        healthSlider.maxValue = stats.Health.MaxValue;
        healthSlider.value = stats.Health.CurrentValue;
        healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
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
