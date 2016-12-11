using UniRx;

namespace Assets.Systems.RoomRotationSystem.Components
{
    public class RoomRotationConfig : GameComponent
    {
        public FloatReactiveProperty Speed = new FloatReactiveProperty(500);
    }
}
