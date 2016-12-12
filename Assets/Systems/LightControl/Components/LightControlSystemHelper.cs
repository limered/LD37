using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Systems.LightControl.Components
{
    public class LightControlSystemHelper : GameComponent
    {
        public List<Light> MainLights;
        public Light StartLight;
        public float LightIntensity;

        public int ChangeDurationMain = 120;
        public int ChangeDurationSingle = 120;

        public void GoToMode(LightMode mode)
        {
            switch (mode)
            {
                case LightMode.Main:
                    MainThreadDispatcher.StartUpdateMicroCoroutine(ChangeMain(true));
                    MainThreadDispatcher.StartUpdateMicroCoroutine(ChangeSingle(false));
                    break;
                case LightMode.Single:
                    MainThreadDispatcher.StartUpdateMicroCoroutine(ChangeMain(false));
                    MainThreadDispatcher.StartUpdateMicroCoroutine(ChangeSingle(true));
                    break;
                case LightMode.Off:
                    MainThreadDispatcher.StartUpdateMicroCoroutine(ChangeMain(false));
                    MainThreadDispatcher.StartUpdateMicroCoroutine(ChangeSingle(false));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode", mode, null);
            }
        }

        private IEnumerator ChangeMain(bool turnOn)
        {
            for (var i = 0; i < ChangeDurationMain; i++)
            {
                if (turnOn)
                {
                    if(MainLights.All(currLight => currLight.intensity < LightIntensity))
                        MainLights.ForEach(currLight => currLight.intensity = (LightIntensity / ChangeDurationMain*i));
                }
                else
                {
                    if (MainLights.All(currLight => currLight.intensity > 0))
                        MainLights.ForEach(currLight => currLight.intensity = LightIntensity - (LightIntensity / ChangeDurationMain * i));
                }
                yield return null;
            }
        }

        private IEnumerator ChangeSingle(bool turnOn)
        {
            for (var i = 0; i < ChangeDurationMain; i++)
            {
                if (turnOn)
                {
                    if(StartLight.intensity < 2f)
                        StartLight.intensity = (2f / ChangeDurationMain * i);
                }
                else
                {
                    if (StartLight.intensity > 0f)
                        StartLight.intensity = 2f - (2f / ChangeDurationMain * i);
                }
                yield return null;
            }
        }
    }
}