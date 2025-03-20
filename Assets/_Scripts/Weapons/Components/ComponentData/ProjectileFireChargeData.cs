using UnityEngine;

namespace Game.Weapons.Components
{
    public class ProjectileFireChargeData : ComponentData<AttackProjectileFireCharge>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ProjectileFireCharge);
        }
    }
}