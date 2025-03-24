
using UnityEngine;

namespace Game.Weapons.Components
{
    public class StatusFireDamageData : ComponentData<AttackStatusFireDamage>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(StatusFireDamage);
        }
    }
}