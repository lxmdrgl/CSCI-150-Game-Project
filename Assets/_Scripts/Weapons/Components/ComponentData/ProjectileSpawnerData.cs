using UnityEngine;

namespace Game.Weapons.Components
{
    public class ProjectileSpawnerData : ComponentData<AttackProjectileSpawner>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(ProjectileSpawner);
        }
    }
}