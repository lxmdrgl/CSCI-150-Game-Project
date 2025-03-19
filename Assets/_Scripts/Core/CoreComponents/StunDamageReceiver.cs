using Game.Combat.StunDamage;
// using Game.ModifierSystem;

namespace Game.CoreSystem
{
    public class StunDamageReceiver : CoreComponent, IStunDamageable
    {
        private Stats stats;

        // public Modifiers<Modifier<StunDamageData>, StunDamageData> Modifiers { get; } = new();

        public void DamageStun(StunDamageData data)
        {
            // data = Modifiers.ApplyAllModifiers(data);
            
            stats.Stun.Decrease(data.Amount);
        }

        protected override void Awake()
        {
            base.Awake();

            stats = core.GetCoreComponent<Stats>();
        }
    }
}