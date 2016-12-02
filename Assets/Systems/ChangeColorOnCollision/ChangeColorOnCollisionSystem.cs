using System;
using System.Collections.Generic;
using Assets.Systems.ChangeColorOnCollision.Components;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Systems.ChangeColorOnCollision
{
    public class ChangeColorOnCollisionSystem : IGameSystem
    {
        public List<Type> SystemComponents { get { return _components; } }
        private readonly List<Type> _components = new List<Type>
        {
            typeof(ChangeColorOnCollisionSystemView),
            typeof(ChangeColorOnCollisionComponent)
        };

        public int Priority { get { return 1; } }

        private Color _color;

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void RegisterComponent(IGameComponent component)
        {
            var viewComp = component as ChangeColorOnCollisionSystemView;
            if (viewComp)
            {
                viewComp.NewColorVector
                    .Subscribe(vec => _color = vec)
                    .AddTo(viewComp);
            }
            var changeColorComp = component as ChangeColorOnCollisionComponent;
            if (changeColorComp)
            {
                changeColorComp
                    .OnCollisionEnterAsObservable()
                    .Subscribe(collision => ChangeColor(changeColorComp))
                    .AddTo(changeColorComp);

                changeColorComp
                    .OnCollisionExitAsObservable()
                    .Subscribe(collision => ChangeColorBack(changeColorComp))
                    .AddTo(changeColorComp);
            }
        }

        private void ChangeColor(ChangeColorOnCollisionComponent self)
        {
            var renderComponent = self.GetComponent<Renderer>();
            self.OldColor = renderComponent.material.color;
            renderComponent.material.color = _color;
        }

        private void ChangeColorBack(ChangeColorOnCollisionComponent self)
        {
            var renderComponent = self.GetComponent<Renderer>();
            renderComponent.material.color = self.OldColor;
        }
    }
}