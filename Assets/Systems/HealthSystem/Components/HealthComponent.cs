using UniRx;

namespace Assets.Systems.HealthSystem.Components
{
    public class HealthComponent : GameComponent
    {
        public float MaxHealth = 100;
        public FloatReactiveProperty CurrentHealth = new FloatReactiveProperty();
    }
}