using Assets.Scripts.Utils;
using Assets.Systems.GameCommands.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assets.Systems.GameCommands
{
    public class GameCommandsSystem : IGameSystem
    {
        public int Priority { get { return 1; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(GameCommandsHelper) }; } }

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as GameCommandsHelper);
        }

        public void RegisterComponent(GameCommandsHelper helper)
        {
            if (!helper) return;
            if (helper.EndGameButton.WasPressed())
                Debug.WriteLine("Close game");
            if (helper.StartGameButton.WasPressed())
                Debug.WriteLine("Start game");
        }
    }
}