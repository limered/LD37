using UniRx;
using UnityEngine;

namespace Assets.Systems.GameControl.Components
{
    public class GameControlHelper : GameComponent
    {
        public ReactiveProperty<GameMode> GameMode = new ReactiveProperty<GameMode>();

        public GameObject Statge0;
        public StageComponent StartStage;

        public void CloseGame()
        {
            Application.Quit();
        }
    }
}