using UnityEngine;

namespace Assets.Systems.RoomRotationSystem.Components
{
    public class RoomRotationWallComponent : GameComponent
    {
        public delegate void RotationRequestedAction(RoomRotationWallComponent comp);
        public event RotationRequestedAction OnRotationRequested;

        public void RotateToThis()
        {
            if (OnRotationRequested != null)
            {
                OnRotationRequested(this);
            }
        }

        public delegate void ResetWallMarks();
        public event ResetWallMarks OnResetWallMarks;

        public void ResetWallmarks()
        {
            if (OnResetWallMarks != null)
            {
                OnResetWallMarks();
            }
        }

        public void Mark()
        {
            var child = transform.GetChild(0);
            var meshRenderer = child.GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
        }
    }
}
