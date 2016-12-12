using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Systems.GameControl;
using Assets.Systems.GameControl.Components;
using UniRx;

namespace Assets.Systems.ButtonAnimation
{
    class ButtonAnimationSystem : IGameSystem
    {
        private ButtonAnimationHelper _helper;
        public int Priority { get { return 20; } }
        public List<Type> SystemComponents { get {return new List<Type> {typeof(ButtonAnimationHelper), typeof(GameControlHelper)}; } }
        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as ButtonAnimationHelper);
            RegisterComponent(component as GameControlHelper);
        }

        private void RegisterComponent(GameControlHelper component)
        {
            if (component)
            {
                component.GameMode
                    .Where(mode => mode == GameMode.Running)
                    .Subscribe(mode => _helper.HideButton())
                    .AddTo(component);
                component.GameMode
                    .Where(mode => mode == GameMode.End)
                    .Subscribe(mode => _helper.ShowButton())
                    .AddTo(component);
            }
        }

        private void RegisterComponent(ButtonAnimationHelper component)
        {
            if (component) _helper = component;
        }
    }
}
