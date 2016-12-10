using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace Assets.Systems.EnemyDeathSystem.Components
{
    public class EnemyDeathOnFallConfig : GameComponent
    {
        public FloatReactiveProperty FallThreshold = new FloatReactiveProperty();
    }
}
