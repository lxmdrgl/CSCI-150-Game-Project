using Game.CoreSystem;
using Game.CoreSystem.StatsSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeSlider;
    public TMP_Text healthText;
    public GameObject player;
    private Stats stats;
    private Death death;
    public float lerpSpeed = 0.05f;

    public float xpos = 0;
    public float ypos = 0;
    public bool showNumbers = true;

    private GameObject gameplayCanvas;
    private RectTransform rectTransform;

    void Awake() {
        gameplayCanvas = GameObject.FindGameObjectWithTag("GameplayCanvas");
        rectTransform = GetComponent<RectTransform>();

        transform.SetParent(gameplayCanvas.transform);
        rectTransform.anchoredPosition = new Vector2(xpos, ypos);
        rectTransform.localScale = new Vector3(1, 1, 1);
    }

    void Start() {
        // SetDependencies();

        healthSlider.maxValue = stats.Health.MaxValue;
        healthSlider.value = stats.Health.CurrentValue;
        if (showNumbers) {
            healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
        }
        easeSlider.maxValue = stats.Health.MaxValue;
        easeSlider.value = stats.Health.CurrentValue;
    }

    public void SetDependencies()
    {
        stats = player.GetComponentInChildren<Stats>();
        death = player.GetComponentInChildren<Death>();
        
        stats.Health.OnValueChange += UpdateSlider;
        if(death != null) {
            death.OnDeath += DisableHealthBar;
        }
    }

    public void SetPlayer(GameObject player) {
        this.player = player;
    }

    void FixedUpdate() {
        if (healthSlider.value != easeSlider.value) {
            easeSlider.value = Mathf.Lerp(easeSlider.value, healthSlider.value, lerpSpeed);
        }
    }

    private void UpdateSlider() {
        healthSlider.maxValue = stats.Health.MaxValue;
        healthSlider.value = stats.Health.CurrentValue;
        easeSlider.maxValue = stats.Health.MaxValue;
        if (showNumbers) {
            healthText.text = healthSlider.value + "/" + healthSlider.maxValue;
        }
    }

    private void DisableHealthBar() {
        // Debug.Log("DisableHealthBar called");
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Debug.Log("HealthBar OnEnable called");
        /* stats.Health.OnValueChange += UpdateSlider;
        if(death != null) {
            death.OnDeath += DisableHealthBar;
        } */
    }

    private void OnDisable()
    {
        if (stats != null) {
            stats.Health.OnValueChange -= UpdateSlider;
        }
        if(death != null) {
            death.OnDeath -= DisableHealthBar;
        }
    }
}
