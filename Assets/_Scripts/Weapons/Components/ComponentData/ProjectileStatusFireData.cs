using UnityEngine;

namespace Game.Weapons.Components
{
    public class ProjectileStatusFireData : ComponentData<AttackStatusFireDamage>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ProjectileStatusFire);
        }
    }
}