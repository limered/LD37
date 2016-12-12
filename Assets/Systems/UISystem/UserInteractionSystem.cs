using Assets.Systems.GameControl;
using Assets.Systems.HealthSystem.Components;
using Assets.Systems.Point;
using Assets.Systems.UISystem.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using Assets.Systems.GameControl.Components;
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

        public List<Type> SystemComponents
        {
            get
            {
                return new List<Type>
                {
                    typeof(UISystemConfig),
                    typeof(UiTriggerComponent),
                    typeof(HealthComponent),
                    typeof(PointsHelper)
                };
            }
        }

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
            RegisterComponent(component as PointsHelper);
            RegisterComponent(component as GameControlHelper);
        }

        #endregion Public Methods

        #region Private Methods

        private void RegisterComponent(GameControlHelper helper)
        {
            if (helper)
            {
                helper.GameMode
                    .Where(mode => mode == GameMode.Running)
                    .Subscribe(mode => HideAllMessages())
                    .AddTo(helper);
            }
        }

        private void HideAllMessages()
        {
            _config.StartCanvas.enabled = false;
            _config.EndCanvas.enabled = false;
        }

        private void RegisterComponent(HealthComponent health)
        {
            if (health && health.tag == "Player")
            {
                health.CurrentHealth
                    .Skip(1)
                    .Subscribe(f => _config.HealthText.text = f.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void RegisterComponent(PointsHelper helper)
        {
            if (helper && _config)
            {
                helper.Points
                    .Subscribe(f => _config.PointsText.text = Math.Floor(f).ToString(CultureInfo.InvariantCulture));
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
            if (GameControlSystem.GameMode == GameMode.StartSequence)
                _config.StartCanvas.enabled = isOn;
            if (GameControlSystem.GameMode == GameMode.End)
                _config.EndCanvas.enabled = isOn;
        }

        #endregion Private Methods
    }
}