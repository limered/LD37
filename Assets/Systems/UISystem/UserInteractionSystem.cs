﻿using Assets.Systems.UISystem.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using Assets.Systems.GameControl;
using Assets.Systems.GameControl.Components;
using Assets.Systems.HealthSystem.Components;
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
        public List<Type> SystemComponents { get { return new List<Type> { typeof(UISystemConfig), typeof(UiTriggerComponent), typeof(HealthComponent) }; } }

        #endregion Public Properties

        #region Public Methods

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as UiTriggerComponent);
            RegisterComponent(component as UISystemConfig);
            RegisterComponent(component as HealthComponent);
        }

        #endregion Public Methods

        #region Private Methods

        private void RegisterComponent(HealthComponent health)
        {
            if (health && health.tag == "Player")
            {
                health.CurrentHealth
                    .Skip(1)
                    .Subscribe(f => _config.HealthText.text = f.ToString(CultureInfo.InvariantCulture));
            }
        }

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
            if(GameControlSystem.GameMode == GameMode.StartSequence)
                _config.StartCanvas.enabled = isOn;
            if(GameControlSystem.GameMode == GameMode.End)
                _config.EndCanvas.enabled = isOn;
        }

        #endregion Private Methods
    }
}