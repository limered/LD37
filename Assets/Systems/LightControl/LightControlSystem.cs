using Assets.Systems.GameControl;
using Assets.Systems.GameControl.Components;
using Assets.Systems.LightControl.Components;
using System;
using System.Collections.Generic;
using Assets.Systems.Lightshow.Events;
using UniRx;

namespace Assets.Systems.LightControl
{
    public enum LightMode
    {
        Main, Single, Off
    }

    internal class LightControlSystem : IGameSystem
    {
        #region Private Fields

        private LightControlSystemHelper _helper;

        #endregion Private Fields

        #region Public Properties

        public int Priority { get { return 20; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(LightControlSystemHelper), typeof(GameControlHelper) }; } }

        #endregion Public Properties

        #region Public Methods

        public void Init()
        {
            MessageBroker.Default.Receive<AlarmEventArgs>().Subscribe(args=>_helper.Alarm(args));
            MessageBroker.Default.Receive<FlickerEventArgs>().Subscribe(args => _helper.Flicker(args));
            MessageBroker.Default.Receive<ChristmasEventArgs>().Subscribe(args => _helper.Christmas(args));
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as LightControlSystemHelper);
            RegisterComponent(component as GameControlHelper);
        }

        #endregion Public Methods

        #region Private Methods

        private bool GameRunningNormal(GameMode mode)
        {
            return mode == GameMode.Running;
        }

        private bool IsInStartOrEnd(GameMode mode)
        {
            return mode == GameMode.StartSequence || mode == GameMode.End;
        }

        private void RegisterComponent(LightControlSystemHelper helper)
        {
            if (helper) _helper = helper;
        }

        private void RegisterComponent(GameControlHelper helper)
        {
            if (!helper) return;
            helper.GameMode
                .Where(GameRunningNormal)
                .Subscribe(SwitchToMain)
                .AddTo(helper);
            helper.GameMode
                .Where(IsInStartOrEnd)
                .Subscribe(SwitchToSingle)
                .AddTo(helper);
        }
        private void SwitchToMain(GameMode mode)
        {
            _helper.GoToMode(LightMode.Main);
        }

        private void SwitchToSingle(GameMode mode)
        {
            if(_helper)
                _helper.GoToMode(LightMode.Single);
        }

        #endregion Private Methods
    }
}