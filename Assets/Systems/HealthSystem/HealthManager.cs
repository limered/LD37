using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Systems.HealthSystem.Components;
using UniRx;

namespace Assets.Systems.HealthSystem
{
    class HealthManager : IGameSystem
    {
        public int Priority { get { return 3; } }
        public List<Type> SystemComponents { get {return new List<Type> {typeof(HealthComponent)};} }
        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            var comp = component as HealthComponent;
            if (comp)
            {
                comp.CurrentHealth.Value = comp.MaxHealth;
                comp
                    .CurrentHealth
                    .Where(CheckForDeath)
                    .Subscribe(_ => Die(comp))
                    .AddTo(comp);
            }
        }

        public bool CheckForDeath(float f)
        {
            return f <= 0;
        }

        public void Die(HealthComponent comp)
        {
            
        }
    }
}
