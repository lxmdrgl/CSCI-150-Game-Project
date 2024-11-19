using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using Game.CoreSystem;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject playerDeathTrigger;

    [SerializeField] private GameObject deathScreenCanvasGO;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deathScreenCanvasGO.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
