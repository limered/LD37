using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Systems.DamagingSystem.Components;
using Assets.Systems.HealthSystem.Components;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Systems.DamagingSystem
{
    class DamageSystem : IGameSystem
    {
        public int Priority { get { return 4; } }
        public List<Type> SystemComponents { get {return new List<Type> {typeof(DealDamageComponent)};} }
        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            var comp = component as DealDamageComponent;
            if (comp)
            {
                comp.OnCollisionEnterAsObservable()
                    .Where(CanDealDamage)
                    .Subscribe(coll => DealDamage(comp, coll))
                    .AddTo(comp);

            }
        }

        private bool CanDealDamage(Collision coll)
        {
            return coll.gameObject.GetComponent<HealthComponent>();
        }

        private void DealDamage(DealDamageComponent self, Collision coll)
        {
            var health = coll.gameObject.GetComponent<HealthComponent>();
            health.CurrentHealth.Value -= self.DamageToHealth;
        }
    }
}
