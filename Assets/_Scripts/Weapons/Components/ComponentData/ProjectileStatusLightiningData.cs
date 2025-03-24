
using UnityEngine;

namespace Game.Weapons.Components
{
    public class ProjectileStatusLightningData : ComponentData<AttackStatusLightningDamage>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ProjectileStatusLightning);
        }
    }
}