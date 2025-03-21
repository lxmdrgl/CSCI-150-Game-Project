namespace Game.Weapons.Components
{
    public class ParryData : ComponentData
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Parry);
        }
    }
}