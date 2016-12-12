using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets.Systems.ButtonAnimation
{
    public class ButtonAnimationHelper : GameComponent
    {
        public GameObject Button;

        public int AnimationDuration;
        public float ShowPosition;
        public float HidePosition;

        public void HideButton()
        {
            MainThreadDispatcher.StartUpdateMicroCoroutine(ChangeButtonPosition(false));
        }

        public void ShowButton()
        {
            MainThreadDispatcher.StartUpdateMicroCoroutine(ChangeButtonPosition(true));
        }

        private IEnumerator ChangeButtonPosition(bool up)
        {
            var step = (ShowPosition - HidePosition) / AnimationDuration;
            step *= up ? 1 : -1;
            for (var i = 0; i < AnimationDuration; i++)
            {
                Button.transform.Translate(0, 0, step);
                yield return null;
            }
        }
    }
}