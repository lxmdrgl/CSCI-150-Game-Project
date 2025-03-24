
using UnityEngine;

namespace Game.Weapons.Components
{
    public class StatusLightningDamageData : ComponentData<AttackStatusLightningDamage>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(StatusLightningDamage);
        }
    }
}