using UnityEngine;

namespace Game.Combat.StunDamage
{
    public class StunDamageData
    {
        public float Amount { get; private set; }
        public GameObject Source { get; private set; }

        public StunDamageData(float amount, GameObject source)
        {
            Amount = amount;
            Source = source;
        }

        public void SetAmount(float amount) => Amount = amount;
    }
}