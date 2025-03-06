
using UnityEngine;

namespace Game.Weapons.Components
{
    public class StunDamageChargeData : ComponentData<AttackStunDamageCharge>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(StunDamageCharge);
        }
    }
}