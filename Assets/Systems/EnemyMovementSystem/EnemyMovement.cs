using Assets.Systems.EnemyMovementSystem.Components;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Systems.EnemyMovementSystem
{
    public class EnemyMovement : IGameSystem
    {
        private float _speed = 500f;
        private string _wallTag = "Stone";
        public int Priority { get { return 10; } }

        public List<Type> SystemComponents
        {
            get
            {
                return new List<Type>
            {
                typeof(EnemyMovementConfig),
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
            var comp = component as EnemyMovementComponent;
            if (comp)
            {
                comp
                    .OnCollisionStayAsObservable()
                    .Where(collision => collision.gameObject.tag == _wallTag)
                    .Subscribe(collision => comp.IsOnWall = true)
                    .AddTo(comp);
                comp
                    .OnCollisionExitAsObservable()
                    .Where(collision => collision.gameObject.tag == _wallTag)
                    .Subscribe(collision => comp.IsOnWall = false)
                    .AddTo(comp);

                comp
                    .FixedUpdateAsObservable()
                    .Where(unit => comp.IsActive && comp.IsOnWall)
                    .Subscribe(unit => MoveEnemy(comp))
                    .AddTo(comp);
                return;
            }
            var config = component as EnemyMovementConfig;
            if (config)
            {
                config
                    .Speed
                    .Subscribe(f => _speed = f)
                    .AddTo(config);
            }
        }

        private void MoveEnemy(EnemyMovementComponent comp)
        {
            var movementVec = comp.TargetPosition.Value - comp.gameObject.transform.position;
            movementVec.y = 0;
            movementVec = movementVec.normalized * _speed;
            var rigid = comp.GetComponent<Rigidbody>();
            rigid.AddForce(movementVec);
        }
    }
}