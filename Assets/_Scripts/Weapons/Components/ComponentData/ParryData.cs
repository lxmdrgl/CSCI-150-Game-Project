using UnityEngine;

namespace Game.Weapons.Components
{
    public class ParryData : ComponentData<AttackParry>
    {
        [field: SerializeField] public LayerMask DetectableLayers { get; private set; }

        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Parry);
        }
    }
}