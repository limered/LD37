using Assets.Systems.DumbKISystem.Components;
using System;
using System.Collections.Generic;
using Assets.Systems.EnemyMovementSystem.Components;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Systems.DumbKISystem
{
    public class DumbKi : IGameSystem
    {
        private GameObject _playerObject;

        public int Priority { get { return 11; } }

        public List<Type> SystemComponents
        {
            get
            {
                return new List<Type>
                {
                    typeof(DumbKiSystemConfig),
                    typeof(EnemyMovementComponent)
                };
            }
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void RegisterComponent(IGameComponent component)
        {
            var config = component as DumbKiSystemConfig;
            if (config)
            {
                _playerObject = config.Player;
                return;
            }
            var comp = component as EnemyMovementComponent;
            if (comp)
            {
                comp
                    .UpdateAsObservable()
                    .Subscribe(unit => SetTargetPosition(comp))
                    .AddTo(comp);
            }
        }

        private void SetTargetPosition(EnemyMovementComponent enemy)
        {
            enemy.TargetPosition.Value = _playerObject.transform.position;
        }
    }
}