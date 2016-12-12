using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace Assets.Systems.EnemyMovementSystem.Components
{
    public class EnemyMovementConfig : GameComponent
    {
        public FloatReactiveProperty Speed = new FloatReactiveProperty(500);
    }
}
