using Assets.Systems.GameControl;
using Assets.Systems.Lightshow.Events;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using Random = UnityEngine.Random;

namespace Assets.Systems.Lightshow
{
    public class LightShowSystem : IGameSystem
    {
        public int Priority { get { return 30; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(LightshowHelper) }; } }

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as LightshowHelper);
        }

        private void RegisterComponent(LightshowHelper helper)
        {
            if (helper)
            {
                helper.FixedUpdateAsObservable().Subscribe(_ => Update()).AddTo(helper);
            }
        }

        private void Update()
        {
            if (GameControlSystem.GameMode == GameMode.Running)
            {
                var useEffect = Random.value;
                if (useEffect < 0.001)
                {
                    var whitch = Random.value;
                    if (whitch < 0.333)
                    {
                        var duration = ((int)Random.value * 300) + 60;
                        var lightNr = (int) (Random.value*7);
                        MessageBroker.Default.Publish(new ChristmasEventArgs { Duration = duration, Nr = lightNr });
                    }
                    else if (whitch < 0.666)
                    {
                        var off = ((int)Random.value * 20) + 5;
                        var on = ((int)Random.value * 60) + 5;
                        MessageBroker.Default.Publish(new FlickerEventArgs { OfTime = off, OnTime = on });
                    }
                    else
                    {
                        var duration = ((int)Random.value * 60) + 30;
                        MessageBroker.Default.Publish(new AlarmEventArgs { Duration = duration });
                    }
                }
            }
        }
    }
}