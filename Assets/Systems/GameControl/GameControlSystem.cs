using Assets.Systems.EnemySpawnerSystem.Components;
using Assets.Systems.GameControl.Components;
using Assets.Systems.GameControl.EventArgs;
using Assets.Systems.HealthSystem.Events;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Systems.GameControl
{
    public enum GameMode
    {
        StartSequence, Running, End
    }

    public class GameControlSystem : IGameSystem
    {
        #region Private Fields

        public static GameMode GameMode = GameMode.StartSequence;
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
            MessageBroker.Default.Receive<DiedArgs>().Subscribe(SomebodyDied);
        }

        private void SomebodyDied(DiedArgs diedArgs)
        {
            if (diedArgs.Comp.tag == "Player")
            {
                GameMode = GameMode.End;
                _helper.GameMode.Value = GameMode.End;
                DeactivateStages();
            }
        }

        private void DeactivateStages()
        {
            ChangeSpanerState(_helper.Statge0, false);
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
            if (GameMode != GameMode.StartSequence && GameMode != GameMode.End) return;

            GameMode = GameMode.Running;
            _helper.GameMode.Value = GameMode;
            ChangeSpanerState(_helper.Statge0, true);
        }

        private void ChangeSpanerState(GameObject parent, bool state)
        {
            var spawners = parent.GetComponentsInChildren<SpawnerComponent>();
            foreach (var spawner in spawners)
            {
                spawner.IsActive = state;
            }
        }

        #endregion Private Methods
    }
}