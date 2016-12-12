using Assets.Systems.GameControl;
using Assets.Systems.GameControl.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Systems.Point
{
    internal class PointsSystem : IGameSystem
    {
        private PointsHelper _helper;
        private IDisposable _timerDisposable;
        public int Priority { get { return 14; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(PointsHelper), typeof(GameControlHelper) }; } }

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as PointsHelper);
            RegisterComponent(component as GameControlHelper);
        }

        private void RegisterComponent(PointsHelper component)
        {
            if (component) _helper = component;
        }

        private void RegisterComponent(GameControlHelper component)
        {
            if (component)
            {
                component.GameMode
                    .Where(mode => mode == GameMode.Running)
                    .Subscribe(mode => StartTimer())
                    .AddTo(component);
                component.GameMode
                    .Where(mode => mode == GameMode.End)
                    .Subscribe(mode => StopTimer())
                    .AddTo(component);
            }
        }

        private void StopTimer()
        {
            if (_timerDisposable != null) _timerDisposable.Dispose();
        }

        private void StartTimer()
        {
            _helper.Points.Value = 0;

            if (_timerDisposable != null) _timerDisposable.Dispose();

            _timerDisposable = _helper
                .FixedUpdateAsObservable()
                .Subscribe(IncrementPoints);
        }

        private void IncrementPoints(Unit u)
        {
            _helper.Points.Value += Time.fixedDeltaTime;
        }
    }
}