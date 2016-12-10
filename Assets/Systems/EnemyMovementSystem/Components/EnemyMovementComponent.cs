using UniRx;
using UnityEditor;

namespace Assets.Systems.EnemyMovementSystem.Components
{
    public class EnemyMovementComponent : GameComponent
    {
        public bool IsOnWall { get; set; }
        public bool IsActive = true;

        public Vector3ReactiveProperty TargetPosition;
    }
}