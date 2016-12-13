using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Systems.Lightshow.Events;
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

        #region Special

        public void Alarm(AlarmEventArgs args)
        {
            MainThreadDispatcher.StartUpdateMicroCoroutine(AlarmLights(args.Duration));
        }

        public void Flicker(FlickerEventArgs args)
        {
            MainThreadDispatcher.StartUpdateMicroCoroutine(FlickerLights(args.OfTime, args.OnTime));
        }

        public void Christmas(ChristmasEventArgs args)
        {
            MainThreadDispatcher.StartUpdateMicroCoroutine(ChristLights(args.Duration, args.Nr));
        }

        #endregion

        private IEnumerator AlarmLights(int duration)
        {
            var single = (int)duration*0.5;
            
            for (var i = 0; i < single; i++)
            {
                MainLights.ForEach(currLight => currLight.intensity = (float) (LightIntensity - (LightIntensity / single * i)));
                yield return null;
            }
            for (var i = 0; i < single; i++)
            {
                MainLights.ForEach(currLight => currLight.intensity = (float)(LightIntensity / single * i));
                yield return null;
            }
        }
        private IEnumerator FlickerLights(int off, int on)
        {
            for (var i = 0; i < off; i++)
            {
                MainLights.ForEach(currLight => currLight.intensity = (float)(LightIntensity - (LightIntensity / off * i)));
                yield return null;
            }
            for (var i = 0; i < on; i++)
            {
                MainLights.ForEach(currLight => currLight.intensity = (float)(LightIntensity / on * i));
                yield return null;
            }
            for (var i = 0; i < (off/2); i++)
            {
                MainLights.ForEach(currLight => currLight.intensity = (float)(LightIntensity - (LightIntensity / (off/2) * i)));
                yield return null;
            }
            for (var i = 0; i < (on/2); i++)
            {
                MainLights.ForEach(currLight => currLight.intensity = (float)(LightIntensity / (on/2) * i));
                yield return null;
            }
        }
        private IEnumerator ChristLights(int duration, int nr)
        {
            for (var i = 0; i < duration; i++)
            {
                //var nr = (int)(UnityEngine.Random.value * MainLights.Count);

                var curr = MainLights[nr];
                if (curr.intensity < LightIntensity) curr.intensity = LightIntensity;
                else if (curr.intensity > 0) curr.intensity = 0;
                yield return null;
            }
            MainLights.ForEach(currLight => currLight.intensity = LightIntensity);
        }
    }
}