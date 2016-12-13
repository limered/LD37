using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;

namespace Assets.Systems.SaveThePlayer
{
    public class TeleportSystem : IGameSystem
    {
        public int Priority { get { return 29; } }
        public List<Type> SystemComponents { get {return new List<Type> {typeof(TeleportOnDropComponent)};} }

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as TeleportOnDropComponent);
        }

        private void RegisterComponent(TeleportOnDropComponent comp)
        {
            if (comp)
            {
                comp.UpdateAsObservable()
                    .Where(unit => MinPositionTriggerHit(comp))
                    .Subscribe(unit => TeleportToTarget(comp))
                    .AddTo(comp);
            }
        }

        private bool MinPositionTriggerHit(TeleportOnDropComponent comp)
        {
            return comp.transform.position.y < comp.MinHeightTrigger;
        }

        private void TeleportToTarget(TeleportOnDropComponent comp)
        {
            comp.transform.position = comp.TeleportLocation.position;
        }
    }
}