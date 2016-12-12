using UniRx;

namespace Assets.Systems.Point
{
    public class PointsHelper : GameComponent
    {
        public FloatReactiveProperty Points = new FloatReactiveProperty(0);
    }
}