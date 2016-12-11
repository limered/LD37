using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Systems.RoomRotationSystem.Components;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Random = System.Random;

namespace Assets.Systems.RoomRotationSystem
{
    public class RoomRotator : IGameSystem
    {
        private RotatableRoomComponent _room;
        private const float DeltaDistance = 5f;

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
        }

        private void OnRotationRequested(RoomRotationWallComponent comp)
        {
            var worldPos = comp.transform.position - _room.transform.position;
            var fwd = _room.transform.forward;
            Debug.Log(string.Format("Wall world position: ({0}|{1}|{2})", fwd.x, fwd.y, fwd.z));

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
            Debug.Log(string.Format("rotate {3} ({0}|{1}|{2})", axis.x, axis.y, axis.z, angle));
            _room.AnimateRotation(axis, angle);
        }
    }
}
