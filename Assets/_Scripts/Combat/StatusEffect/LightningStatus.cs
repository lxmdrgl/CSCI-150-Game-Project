using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.CoreSystem;
using UnityEngine;

namespace Game.Combat.Status
{
    public class LightningStatus : StatusData
    {
        public int Ticks { get; private set;}
        public float Delay { get; private set;}
        public int Count { get; private set;}
        public float Radius { get; private set;}
        private LayerMask WhatIsDamageable;
        public int tickIndex { get; private set;}
        public int countIndex { get; private set;}
        public int coroutineCount { get; private set;}
        public event Action OnComplete;

        public LightningStatus(float amount, float damage, float stun, GameObject source, int ticks, float delay, int count, float radius, LayerMask whatIsDamageable) : base (amount, damage, stun, source)
        {
            Ticks = ticks;
            Delay = delay;
            Count = count;
            Radius = radius;
            WhatIsDamageable = whatIsDamageable;
        }

        public override void ApplyStatus(Stats stats, Action onCompleteCallback)
        {
            base.ApplyStatus(stats, onCompleteCallback);

            OnComplete = onCompleteCallback;

            coroutineCount = 0;

            if (Stats.isActiveAndEnabled)
            {
                Stats.GetComponent<MonoBehaviour>().StartCoroutine(SpreadStatus());
                Stats.GetComponent<MonoBehaviour>().StartCoroutine(ApplyStatusOverTime());
            }

        }

        private IEnumerator ApplyStatusOverTime()
        {
            for (tickIndex = 1; tickIndex < Ticks; tickIndex++)
            {
                yield return new WaitForSeconds(Delay);

                if (Stats.isActiveAndEnabled)
                {
                    Stats.GetComponent<MonoBehaviour>().StartCoroutine(SpreadStatus());
                }
            }
            OnComplete?.Invoke();
        }

        private IEnumerator SpreadStatus()
        {
            List<Stats> currentStats = new List<Stats>{Stats};

            currentStats[0].Health.Decrease(Damage);
            currentStats[0].Stun.Decrease(Stun);
            Debug.Log($"Apply Lightning status first: {Damage}, {Stun}");

            Vector2 currentPosition = currentStats[0].transform.position;
            for (countIndex = 0; countIndex < Count - 1; countIndex++) 
            {
                List<Collider2D> detectedTargets = Physics2D.OverlapCircleAll(currentPosition, Radius, WhatIsDamageable).ToList();

                Collider2D nearestTarget = null;
                float minDistance = float.MaxValue;
                foreach (Collider2D detected in detectedTargets)
                {
                    if (!currentStats.Contains(detected.gameObject.GetComponentInChildren<Stats>()))
                    {
                        Vector2 targetPosition = detected.transform.position;
                        float distance = (targetPosition - currentPosition).sqrMagnitude;

                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestTarget = detected;
                        }
                    }
                }

                if (nearestTarget != null)
                {
                    yield return new WaitForSeconds(0.2f);

                    Stats newStats = nearestTarget.gameObject.GetComponentInChildren<Stats>();
                    newStats.Health.Decrease(Damage);
                    newStats.Stun.Decrease(Stun);
                    currentPosition = newStats.transform.position;
                    currentStats.Add(newStats);
                    Debug.Log($"Apply Lightning status spread: {Damage}, {Stun}, {countIndex}");
                }
            }
        }

        public override void ReapplyStatus(StatusData data)
        {
            if (data is LightningStatus lightningData)
            {
                Damage = lightningData.Damage;
                Stun = lightningData.Stun;
                tickIndex = 0;
                countIndex = 0;
                Ticks = lightningData.Ticks;
                Delay = lightningData.Delay;
                Count = lightningData.Count;

                Debug.Log($"Reapply status Lightning: {Damage}, {Stun}, {Ticks}, {Delay}, {Count}, {countIndex}");
            }
            else
            {
                Debug.LogError("Wrong status data type");
            }
        }
    }
}