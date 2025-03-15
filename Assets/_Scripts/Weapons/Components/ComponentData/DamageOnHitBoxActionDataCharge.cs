namespace Game.Weapons.Components
{
    public class DamageOnHitBoxActionChargeData : ComponentData<AttackDamageCharge>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(DamageOnHitBoxActionCharge);
        }
    }
}