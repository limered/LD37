using Assets.Systems.EnemyDeathSystem.Components;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Systems.EnemyDeathSystem
{
    public class EnemyDeath : IGameSystem
    {
        private float _fallThreshold;
        public int Priority { get { return 2; } }

        public List<Type> SystemComponents
        {
            get
            {
                return new List<Type>
        {
            typeof(EnemyDeathOnFallConfig),
            typeof(DieOnFallComponent)
        };
            }
        }

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            var config = component as EnemyDeathOnFallConfig;
            if (config)
            {
                config.FallThreshold
                    .Subscribe(f => _fallThreshold = f)
                    .AddTo(config);
                return;
            }
            var comp = component as DieOnFallComponent;
            if (comp)
            {
                comp
                    .OnTriggerEnterAsObservable()
                    .Where(coll => CheckForDeath(comp))
                    .Subscribe(coll => Die(comp))
                    .AddTo(comp);
            }
        }

        private bool CheckForDeath(DieOnFallComponent comp)
        {
            var rigid = comp.GetComponent<Rigidbody>();
            return rigid.velocity.y < _fallThreshold;
        }

        private void Die(DieOnFallComponent comp)
        {
            // ReSharper disable once AccessToStaticMemberViaDerivedType
            GameObject.Destroy(comp.gameObject);
        }
    }
}