using System;
using System.Collections.Generic;
using Assets.Systems.GameControl.Components;
using Assets.Systems.RoomRotationSystem.Components;
using Assets.Systems.RoomRotationSystem.Events;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace Assets.Systems.RoomRotationSystem
{
    public class RoomRotator : IGameSystem
    {
        private RotatableRoomComponent _room;
        private const float DeltaDistance = 5f;
        private float _speed = 300;

        public int Priority
        {
            get { return 2; }
        }

        public List<Type> SystemComponents
        {
            get
            {
                return new List<Type>
                {
                    typeof(RoomRotationConfig),
                    typeof(RoomRotationWallComponent),
                    typeof(RotatableRoomComponent),
                    typeof(GameControlHelper)
                };
            }
        }

        public void Init()
        {
            MessageBroker.Default.Receive<RoomRotationResetArgs>().Subscribe(ResetRoomRotation);
        }

        private void ResetRoomRotation(RoomRotationResetArgs roomRotationResetArgs)
        {
            _room.transform.localRotation = Quaternion.identity;
        }

        public void RegisterComponent(IGameComponent component)
        {
            
            RegisterComponent(component as RoomRotationWallComponent);
            RegisterComponent(component as RotatableRoomComponent);
            RegisterComponent(component as RoomRotationConfig);
            RegisterComponent(component as GameControlHelper);
        }

        private void RegisterComponent(RoomRotationWallComponent comp)
        {
            if(comp) comp.OnRotationRequested += OnRotationRequested;
        }

        private void RegisterComponent(RotatableRoomComponent comp)
        {
            if (comp) _room = comp;
        }

        private void RegisterComponent(RoomRotationConfig comp)
        {
            if (comp) {
                comp
                    .Speed
                    .Subscribe(f => _speed = f)
                    .AddTo(comp);
            }
        }

        private void RegisterComponent(GameControlHelper comp)
        {
            if (comp)
            {
                comp.GameMode
                    .Skip(1)
                    .Subscribe(_ => _room.transform.localRotation = Quaternion.identity);
            }
        }


        private void OnRotationRequested(RoomRotationWallComponent comp)
        {
            var worldPos = comp.transform.position - _room.transform.position;

            var angle = 90;
            Vector3 axis;
            if (worldPos.x <= DeltaDistance*-1)
            {
                axis = Vector3.forward;
            }
            else if (worldPos.x >= DeltaDistance)
            {
                axis = Vector3.back;
            }
            else if (worldPos.y <= DeltaDistance*-1)
            {
                //floor, do nothing
                return;
            }
            else if (worldPos.y >= DeltaDistance)
            {
                //ceiling, rotate random 180 degrees right, left, front or back
                var random = new Random();
                        angle = 180;

                switch (random.Next(0, 3))
                {
                    case 0:
                        axis = Vector3.forward;
                        break;
                    case 1:
                        axis = Vector3.back;
                        break;
                    case 2:
                        axis = Vector3.left;
                        break;
                    case 3:
                        axis = Vector3.right;
                        break;
                    default:
                        return;
                }
            }
            else if (worldPos.z <= DeltaDistance * -1)
            {
                axis = Vector3.left;
            }
            else if (worldPos.z >= DeltaDistance)
            {
                axis = Vector3.right;
            }
            else
            {
                return;
            }
            _room.AnimateRotation(axis, angle, _speed);
        }
    }
}
