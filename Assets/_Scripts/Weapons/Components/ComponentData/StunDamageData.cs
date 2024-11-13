
namespace Game.Weapons.Components
{
    public class StunDamageData : ComponentData<AttackStunDamage>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(StunDamage);
        }
    }
}