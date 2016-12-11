using System;
using System.Collections.Generic;
using Assets.Systems.RoomRotationSystem.Components;

namespace Assets.Systems.RoomRotationSystem
{
    public class RoomRotator : IGameSystem
    {
        private RotatableRoomComponent _room;

        public int Priority
        {
            get { return 10; }
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
            //ToDo #7: rotate room
        }
    }
}
