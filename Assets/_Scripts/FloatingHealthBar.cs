using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.CoreSystem;
using Game.CoreSystem.StatsSystem;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    [SerializeField] public Stats stats;
    
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
