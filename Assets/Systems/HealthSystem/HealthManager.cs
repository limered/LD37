using Assets.Systems.HealthSystem.Components;
using Assets.Systems.HealthSystem.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Systems.GameControl;
using Assets.Systems.GameControl.Components;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Systems.HealthSystem
{
    internal class HealthManager : IGameSystem
    {
        public int Priority { get { return 3; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(HealthComponent), typeof(GameControlHelper) }; } }

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as HealthComponent);
            RegisterComponent(component as GameControlHelper);
        }

        private void RegisterComponent(GameControlHelper helper)
        {
            if (!helper) return;
            helper.GameMode
                .Where(mode => mode == GameMode.Running)
                .Subscribe(mode => ResetHealthComponents())
                .AddTo(helper);
        }

        private void ResetHealthComponents()
        {
            HealthComponent[] comps = GameObject.FindObjectsOfType(typeof(HealthComponent)) as HealthComponent[];
            if(comps == null)return;
            foreach (var comp in comps)
            {
                comp.CurrentHealth.Value = comp.MaxHealth;
            }
        }

        private void RegisterComponent(HealthComponent comp)
        {
            if (!comp) return;
            comp.CurrentHealth.Value = comp.MaxHealth;
            comp
                .CurrentHealth
                .Where(CheckForDeath)
                .Subscribe(_ => Die(comp))
                .AddTo(comp);
        }

        public bool CheckForDeath(float f)
        {
            return f <= 0;
        }

        public void Die(HealthComponent comp)
        {
            MessageBroker.Default.Publish(new DiedArgs { Comp = comp });
        }
    }
}