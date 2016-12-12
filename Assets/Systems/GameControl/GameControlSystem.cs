using Assets.Systems.EnemySpawnerSystem.Components;
using Assets.Systems.GameControl.Components;
using Assets.Systems.GameControl.EventArgs;
using Assets.Systems.HealthSystem.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Systems.GameControl
{
    public enum GameMode
    {
        StartSequence, Running, Stage2, Stage3, End
    }

    public class GameControlSystem : IGameSystem
    {
        #region Private Fields

        public static GameMode GameMode = GameMode.StartSequence;
        private GameControlHelper _helper;

        #endregion Private Fields

        #region Public Properties

        public int Priority { get { return 1; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(GameControlHelper), typeof(StageComponent) }; } }

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
                ResetStages();
            }
        }

        private void ResetStages()
        {
            var stage = _helper.StartStage;
            do
            {
                stage.IsActive = false;
                stage.IsDone.Value = false;
                stage = stage.nextStage;

            } while (stage.nextStage);
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as GameControlHelper);
            RegisterComponent(component as StageComponent);
        }

        #endregion Public Methods

        #region Private Methods

        private void RegisterComponent(StageComponent stage)
        {
            if (stage)
            {
                stage
                    .UpdateAsObservable()
                    .Where(unit => stage.IsActive)
                    .Where(unit => CheckStageForDone(stage))
                    .Subscribe(unit => stage.IsDone.Value = true)
                    .AddTo(stage);

                stage.IsDone
                    .Where(b => b)
                    .Subscribe(_ => StartNextStage(stage))
                    .AddTo(stage);
            }
        }

        private bool CheckStageForDone(StageComponent stage)
        {
            SpawnerComponent[] spawners = stage.GetComponentsInChildren<SpawnerComponent>();

            return spawners == null || spawners.All(component => component.IsActive == false);
        }

        private void StartNextStage(StageComponent current)
        {
            current.IsActive = false;
            ChangeSpawnerState(current.gameObject, false);
            ChangeSpawnerState(current.nextStage.gameObject, true);
            current.nextStage.IsActive = true;
        }

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
            ChangeSpawnerState(_helper.StartStage.gameObject, true);
            _helper.StartStage.IsActive = true;
        }

        private void ChangeSpawnerState(GameObject parent, bool state)
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