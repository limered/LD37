using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Systems
{
    public class GameComponent : MonoBehaviour, IGameComponent
    {
        protected void Start()
        {
            RegisterToGame();
        }

        public void RegisterToGame()
        {
            IoC.Resolve<Game.Game>().RegisterComponent(this);
        }
    }
}