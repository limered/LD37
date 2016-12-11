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
    }
}
