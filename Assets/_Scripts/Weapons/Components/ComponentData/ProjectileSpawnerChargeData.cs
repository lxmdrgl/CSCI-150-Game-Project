using UnityEngine;

namespace Game.Weapons.Components
{
    public class ProjectileSpawnerChargeData : ComponentData<AttackProjectileSpawnerCharge>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ProjectileSpawnerCharge);
        }
    }
}