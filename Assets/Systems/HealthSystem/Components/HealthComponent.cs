using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace Assets.Systems.HealthSystem.Components
{
    class HealthComponent : GameComponent
    {
        public float MaxHealth;
        public FloatReactiveProperty CurrentHealth = new FloatReactiveProperty();
    }
}
