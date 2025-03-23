using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Game.Combat.Status;
using Game.CoreSystem.StatsSystem;

namespace Game.CoreSystem
{
    public class StatusDamageReceiver : CoreComponent, IStatusDamageable
    {
        private Stats stats;

        private List<StatusData> statusEffects = new List<StatusData>();

        public void StatusDamage(StatusData data)
        {
            UnityEngine.Debug.Log("Status data: " + data);
            // AddStatus(data);

            var existingStatus = stats.Statuses.FirstOrDefault(s => s.Item2.GetType() == data.GetType());

            // Create new stat in Statuses list
            if (existingStatus.Item2 == null)
            {
                Stat newStat = new Stat();
                newStat.MaxValue = stats.Status;
                newStat.Init();

                stats.Statuses.Add((newStat, data));
                UnityEngine.Debug.Log("Create new Status bar");
            }

            // Find stat in Statuses that matches type, and decrease Amount

            int index = stats.Statuses.FindIndex(s => s.Item2.GetType() == data.GetType());
            
            stats.Statuses[index].Item1.Decrease(data.Amount);
            UnityEngine.Debug.Log($"Decrease status bar: {stats.Statuses[index].Item1.CurrentValue}, {data.Amount}");

            // If status is 0
            if (stats.Statuses[index].Item1.CurrentValue >= -0.01f && stats.Statuses[index].Item1.CurrentValue <= 0.01f)
            {
                UnityEngine.Debug.Log($"Reset status bar, adding {data.GetType()}");
                stats.Statuses[index].Item1.CurrentValue = stats.Statuses[index].Item1.MaxValue;
                AddStatus(data);
            }
        }

        public void AddStatus(StatusData data)
        {
            int index = statusEffects.FindIndex(s => s.GetType() == data.GetType());

            UnityEngine.Debug.Log("Status effects " + index + ": " + string.Join(", ", statusEffects));
            if (index >= 0)
            {
                UnityEngine.Debug.Log($"Reapply status: {index} {data.GetType()}");
                statusEffects[index].ReapplyStatus(data);
            }
            else
            {
                statusEffects.Add(data);
                index = statusEffects.FindIndex(s => s.GetType() == data.GetType());

                UnityEngine.Debug.Log($"Apply status: {index} {data.GetType()}");
                statusEffects[index].ApplyStatus(stats, () => RemoveStatus(data));
            }
        }

        private void RemoveStatus(StatusData data)
        {
            if (statusEffects.Contains(data))
            {
                UnityEngine.Debug.Log($"Removed status: {data.GetType().Name}");
                statusEffects.Remove(data);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            stats = core.GetCoreComponent<Stats>();
        }
    }
}