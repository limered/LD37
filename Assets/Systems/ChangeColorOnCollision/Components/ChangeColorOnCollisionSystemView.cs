using UniRx;

namespace Assets.Systems.ChangeColorOnCollision.Components
{
    public class ChangeColorOnCollisionSystemView : GameComponent
    {
        public ColorReactiveProperty NewColorVector = new ColorReactiveProperty();

        private new void Start()
        {
            base.Start();
        }
    }
}