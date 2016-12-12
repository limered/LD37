using Assets.Systems.UISystem.Components;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;

namespace Assets.Systems.UISystem
{
    internal class UserInteractionSystem : IGameSystem
    {
        #region Private Fields

        private UISystemConfig _config;

        #endregion Private Fields

        #region Public Properties

        public int Priority { get { return 15; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(UISystemConfig), typeof(UiTriggerComponent) }; } }

        #endregion Public Properties

        #region Public Methods

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as UiTriggerComponent);
            RegisterComponent(component as UISystemConfig);
        }

        #endregion Public Methods

        #region Private Methods

        private void RegisterComponent(UISystemConfig config)
        {
            if (!config) return;
            _config = config;
        }

        private void RegisterComponent(UiTriggerComponent uiTrigger)
        {
            if (!uiTrigger) return;
            uiTrigger
                .OnTriggerEnterAsObservable()
                .Subscribe(_ => ToggleUseMessage(true))
                .AddTo(uiTrigger);
            uiTrigger
                .OnTriggerExitAsObservable()
                .Subscribe(_ => ToggleUseMessage(false))
                .AddTo(uiTrigger);
        }

        private void ToggleUseMessage(bool isOn)
        {
            _config.StartCanvas.enabled = isOn;
        }

        #endregion Private Methods
    }
}