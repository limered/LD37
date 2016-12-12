using Assets.Systems.HealthSystem.Components;

namespace Assets.Systems.HealthSystem.Events
{
    public class DiedArgs
    {
        public HealthComponent Comp { get; set; }
    }
}