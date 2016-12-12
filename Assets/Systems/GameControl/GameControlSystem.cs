using Assets.Systems.GameControl.Components;
using Assets.Systems.GameControl.EventArgs;
using System;
using System.Collections.Generic;
using UniRx;

namespace Assets.Systems.GameControl
{
    public enum GameMode
    {
        StartSequence, Running, End
    }

    public class GameControlSystem : IGameSystem
    {
        #region Private Fields

        private GameMode _gameMode = GameMode.StartSequence;
        private GameControlHelper _helper;

        #endregion Private Fields

        #region Public Properties

        public int Priority { get { return 1; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(GameControlHelper) }; } }

        #endregion Public Properties

        #region Public Methods

        public void Init()
        {
            MessageBroker.Default.Receive<GameStartArgs>().Subscribe(StartGame);
            MessageBroker.Default.Receive<GameCloseArgs>().Subscribe(CloseGame);
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as GameControlHelper);
        }

        #endregion Public Methods

        #region Private Methods

        private void CloseGame(GameCloseArgs args)
        {
            _helper.CloseGame();
        }

        private void RegisterComponent(GameControlHelper helper)
        {
            if (!helper) return;
            _helper = helper;
        }

        private void StartGame(GameStartArgs gameStartArgs)
        {
            if (_gameMode != GameMode.StartSequence) return;

            _gameMode = GameMode.Running;
            _helper.GameMode.Value = _gameMode;
            _helper.Statge0.SetActive(true);
        }

        #endregion Private Methods
    }
}