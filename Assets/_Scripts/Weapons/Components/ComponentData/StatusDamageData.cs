
using UnityEngine;

namespace Game.Weapons.Components
{
    public class StatusDamageData : ComponentData<FireStatusDamage>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(StatusDamage);
        }
    }
}