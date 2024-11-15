
using UnityEngine;

namespace Game.Weapons.Components
{
    public class StunDamageData : ComponentData<AttackStunDamage>
    {
        private float amount;
        private GameObject root;

        public StunDamageData(float amount, GameObject root)
        {
            this.amount = amount;
            this.root = root;
        }

        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(StunDamage);
        }
    }
}