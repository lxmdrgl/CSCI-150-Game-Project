namespace Game.Weapons.Components
{
    public class ChargeData : ComponentData<AttackCharge>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Charge);
        }
    }
}