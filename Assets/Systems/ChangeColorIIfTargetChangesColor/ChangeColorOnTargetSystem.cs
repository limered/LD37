using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Systems.ChangeColorIIfTargetChangesColor.Components;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Systems.ChangeColorIIfTargetChangesColor
{
    public class ChangeColorOnTargetSystem : IGameSystem
    {
        public int Priority { get { return 2; } }

        public List<Type> SystemComponents
        {
            get
            {
                return new List<Type>
                {
                    typeof(ChangeColorOnTargetComponent),
                    typeof(TargetComponent)
                };
            }
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void RegisterComponent(IGameComponent component)
        {
            var comp = component as ChangeColorOnTargetComponent;
            if (comp)
            {
                var target = comp.TargetObject.GetComponent<TargetComponent>() as TargetComponent;
                if (!target) return;

                target.OnCollisionEnterAsObservable()
                    .Subscribe(collision => RegisterToTarget(comp, target))
                    .AddTo(target);
            }
        }

        private void RegisterToTarget(ChangeColorOnTargetComponent self, TargetComponent target)
        {
            var renderer = self.GetComponent<Renderer>();
            renderer.material.color = Color.blue;
        }
    }
}
