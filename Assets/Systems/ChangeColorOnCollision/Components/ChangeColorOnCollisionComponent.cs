using UnityEngine;

namespace Assets.Systems.ChangeColorOnCollision.Components
{
    public class ChangeColorOnCollisionComponent : GameComponent
    {
        public Color OldColor { get; set; }
        private new void Start()
        {
            base.Start();
        }
    }
}