using System.Collections;
using UnityEngine;

namespace Assets.Systems.UISystem.Components
{
    public class UISystemConfig : GameComponent
    {
        public Light MainLight;

        public Canvas StartCanvas;
        public GameObject ButtonModel;
        public Light ButtonLight;

        public void StartGame()
        {
            StartCoroutine(ToggleStartButton(false));
        }

        public void EndGame()
        {
            StartCoroutine(ToggleStartButton(true));
        }

        IEnumerator ToggleStartButton(bool isOn)
        {
            if (!isOn)
            {
                for (var i = 0; i < 120; i++)
                {
                    ButtonLight.intensity = 2f - (2f/120f*i);
                    MainLight.intensity = (1f/120f)*i;
                    var oldPos = ButtonModel.transform.position;
                    ButtonModel.transform.position = new Vector3(0, oldPos.y - (12f/120f), 0);
                    yield return null;
                }
            }
        }
    }
}