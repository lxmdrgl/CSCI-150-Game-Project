using System;
using Game.Combat.Damage;
using Game.ProjectileSystem.Components;
using UnityEngine;

namespace Game.ProjectileSystem
{
    /*
     * This MonoBehaviour is simply used to print the damage amount received in the ProjectileTestScene
     */
    public class TestDamageable : MonoBehaviour, IDamageable
    {
        public void Damage(DamageData data)
        {
            print($"{gameObject.name} Damaged: {data.Amount}");
        }
    }
}