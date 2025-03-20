using UnityEngine;

namespace Game.Weapons.Components
{
    public class ProjectileFireData : ComponentData<AttackProjectileFire>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ProjectileFire);
        }
    }
}