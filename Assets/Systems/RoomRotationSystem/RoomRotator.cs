using System;
using System.Collections.Generic;
using Assets.Systems.RoomRotationSystem.Components;
using UniRx;
using UnityEngine;
using Random = System.Random;

namespace Assets.Systems.RoomRotationSystem
{
    public class RoomRotator : IGameSystem
    {
        private RotatableRoomComponent _room;
        private const float DeltaDistance = 5f;
        private float _speed = 1;

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
                    typeof(RotatableRoomComponent)
                };
            }
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void RegisterComponent(IGameComponent component)
        {
            var comp = component as RoomRotationWallComponent;
            if (comp)
            {
                comp.OnRotationRequested += OnRotationRequested;
                return;
            }

            var room = component as RotatableRoomComponent;
            if (room)
            {
                _room = room;
            }

            var config = component as RoomRotationConfig;
            if (config)
            {
                config
                    .Speed
                    .Subscribe(f => _speed = f)
                    .AddTo(config);
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
