using UnityEngine;
using System;

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
    
        public void Die()
        {
            /* foreach (var particle in deathParticles)
            {
                ParticleManager.StartParticles(particle);
            } */

            OnDeath?.Invoke();
        
            core.transform.parent.gameObject.SetActive(false);

            // Invoke Player Death Screen
            if (core.transform.parent.gameObject.tag == "Player") {
                Debug.Log("Player died");

                deathScreen = GameObject.FindGameObjectWithTag("Deathscreen");
                deathScreen.transform.GetChild(0).gameObject.SetActive(true);
            }

            // Invoke Boss Death Portal
            if (core.transform.parent.gameObject.name == "Boss1") {
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