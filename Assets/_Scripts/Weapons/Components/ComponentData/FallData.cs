namespace Game.Weapons.Components
{
    public class FallData : ComponentData<AttackFall>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Fall);
        }
    }
}