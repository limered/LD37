using Assets.Scripts.Utils;
using Assets.Systems.GameCommands.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Systems.GameControl.EventArgs;
using Assets.Systems.Music;
using Assets.Systems.RoomRotationSystem.Events;
using UniRx;
using UniRx.Triggers;

namespace Assets.Systems.GameCommands
{
    public class GameCommandsSystem : IGameSystem
    {
        public int Priority { get { return 2; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(GameCommandsHelper) }; } }

        public void Init()
        {
            Debug.WriteLine("test");
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as GameCommandsHelper);
        }

        public void RegisterComponent(GameCommandsHelper helper)
        {
            if (!helper) return;
            helper.UpdateAsObservable()
                .Subscribe(_=>CheckForButtons(helper))
                .AddTo(helper);
            
        }

        private void CheckForButtons(GameCommandsHelper helper)
        {
            if (helper.EndGameButton.WasPressed())
                MessageBroker.Default.Publish(new GameCloseArgs());
            if (helper.StartGameButton.WasPressed())
                MessageBroker.Default.Publish(new GameStartArgs());
            if(helper.ResetRoomButton.WasPressed())
                MessageBroker.Default.Publish(new RoomRotationResetArgs());
            if(helper.MuteMusicButton.WasPressed())
                MessageBroker.Default.Publish(new ChangeMusicArgs());
        }
    }
}