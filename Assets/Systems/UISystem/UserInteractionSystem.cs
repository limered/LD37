using Assets.Systems.UISystem.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Systems.UISystem
{
    enum GameMode
    {
        Start, Running, End
    }
    internal class UserInteractionSystem : MonoBehaviour, IGameSystem
    {
        private UISystemConfig _config;

        private GameMode _mode = GameMode.Start;
        public int Priority { get { return 15; } }
        public List<Type> SystemComponents { get { return new List<Type> {typeof(UISystemConfig), typeof(UiTriggerComponent)}; } }

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            var uiTrigger = component as UiTriggerComponent;
            if (uiTrigger)
            {
                uiTrigger
                    .OnTriggerEnterAsObservable()
                    .Subscribe(_ => ToggleUseMessage(true))
                    .AddTo(uiTrigger);
                uiTrigger
                    .OnTriggerExitAsObservable()
                    .Subscribe(_ => ToggleUseMessage(false))
                    .AddTo(uiTrigger);
                uiTrigger
                    .UpdateAsObservable()
                    .Where(_ => _mode == GameMode.Start)
                    .Subscribe(_ => ListenToPressE())
                    .AddTo(uiTrigger);
                return;
            }
            var config = component as UISystemConfig;
            if (config)
            {
                _config = config;
                _config
                    .UpdateAsObservable()
                    .Subscribe(_ => ListenForEsc())
                    .AddTo(_config);
                return;
            }
        }

        private void ToggleUseMessage(bool isOn)
        {
            _config.StartCanvas.enabled = isOn;
        }

        private void ListenToPressE()
        {
            if (KeyCode.E.WasPressed())
            {
                _mode = GameMode.Running;
                _config.StartGame();
            }
        }

        private void ListenForEsc()
        {
            if (KeyCode.Escape.WasPressed())
            {
                Application.Quit();
            }
        }
        
    }
}