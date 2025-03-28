using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

namespace Game.CoreSystem
{
    public class Death : CoreComponent
    {
        // [SerializeField] private GameObject[] deathParticles;

        /* private ParticleManager ParticleManager =>
            particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    
        private ParticleManager particleManager; */

        private GameObject deathScreen;
        private GameObject portal1;

        private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
        private Stats stats;
        // public GameObject healthBar;

        public event Action OnDeath;

        public int Source;
    
        public void Die()
        {
            /* foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            } */

            OnDeath?.Invoke();
        
            core.transform.parent.gameObject.SetActive(false);

            // Check if the current object is a player
            if (core.transform.parent.gameObject.tag == "Player")
            {
                // Check if all players are inactive
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                bool allPlayersDead = true;

                foreach (GameObject player in players)
                {
                    if (player.activeSelf) // If any player is still active
                    {
                        allPlayersDead = false;
                        break;
                    }
                }

                // If all players are dead, show the death screen
                if (allPlayersDead)
                {
                    deathScreen = GameObject.FindGameObjectWithTag("Deathscreen");
                    if (deathScreen != null)
                    {
                        deathScreen.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    Time.timeScale = 0;
                }
            }
            else
            {
                if (Source == 0)
                {
                    PlayerPrefs.SetInt("player1Kills", PlayerPrefs.GetInt("player1Kills") + 1);
                }
                else if (Source == 1)
                {
                    PlayerPrefs.SetInt("player2Kills", PlayerPrefs.GetInt("player2Kills") + 1);
                }
            }
            // Invoke Boss Death Portal
            if (core.transform.parent.gameObject.name == "Boss1") 
            {
                Debug.Log("Boss1 died");
                portal1 = GameObject.FindGameObjectWithTag("Portal1");
                portal1.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        private void OnEnable()
        {
            Stats.Health.OnCurrentValueZero += Die;
        }

        private void OnDisable()
        {
            Stats.Health.OnCurrentValueZero -= Die;
        }
    }
}