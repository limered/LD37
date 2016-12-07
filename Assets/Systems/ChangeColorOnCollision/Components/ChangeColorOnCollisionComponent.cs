using UnityEngine;

namespace Assets.Systems.ChangeColorOnCollision.Components
{
    public class ChangeColorOnCollisionComponent : GameComponent
    {
        public Color OriginalColor = default(Color);
        private new void Start()
        {
            base.Start();
        }
    }
}