namespace Assets.Scripts
{
    using Utils;
    using UniRx;
    using UnityEngine;

    public class Settings
    {
        public const string ThisKey = "This";
        public const string ThatKey = "That";
        public const string ThisOrThatKey = "ThisOrThat";
        public const string BoolFlagKey = "BoolFlag";
        public const string SensitivityKey = "Sensitivity";
        public const string SoundEnabledKey = "SoundEnabled";
        public const string MusicEnabledKey = "MusicEnabled";

        private readonly ReactiveProperty<bool> boolFlag = new BoolReactiveProperty(PlayerPrefs.GetInt(BoolFlagKey, 0) != 0);
        public IObservable<bool> OnBoolFlagChanged { get { return boolFlag; } }
        private readonly ReactiveProperty<ThisOrThat> thisOrThat = new ReactiveProperty<ThisOrThat>(PlayerPrefs.GetString(ThisOrThatKey, ThisKey) == ThisKey ? ThisOrThat.This : ThisOrThat.That);
        public IObservable<ThisOrThat> OnThisOrThatChanged { get { return thisOrThat; } }
        private readonly ReactiveProperty<float> sensitivity = new ReactiveProperty<float>(PlayerPrefs.GetFloat(SensitivityKey, 50f));
        public IObservable<float> OnSensitivityChanged { get { return sensitivity; } }
        private readonly ReactiveProperty<bool> soundEnabled = new BoolReactiveProperty(PlayerPrefs.GetInt(SoundEnabledKey, 1) != 0);
        public IObservable<bool> OnSoundChanged { get { return soundEnabled; } }
        private readonly ReactiveProperty<bool> musicEnabled = new BoolReactiveProperty(PlayerPrefs.GetInt(MusicEnabledKey, 1) != 0);
        public IObservable<bool> OnMusicChanged { get { return musicEnabled; } }

        public bool BoolFlag
        {
            get
            {
                return boolFlag.Value;
            }
            set
            {
                if (BoolFlag == value) return;

                PlayerPrefs.SetInt(BoolFlagKey, value ? 1 : 0);
                PlayerPrefs.Save();
                boolFlag.Value = value;
                Debug.Log("Setting: " + "BoolFlag" + " set to " + value);
            }
        }

        public ThisOrThat ControllerType
        {
            get
            {
                return thisOrThat.Value;
            }
            set
            {
                if (ControllerType == value) return;

                string tt;
                switch (value)
                {
                    case ThisOrThat.This: tt = ThisKey; break;
                    case ThisOrThat.That: tt = ThatKey; break;
                    default: tt = ThisKey; break;
                }

                PlayerPrefs.SetString(ThisOrThatKey, tt);
                PlayerPrefs.Save();
                thisOrThat.Value = value;
                Debug.Log("Setting: " + "ThisOrThat" + " set to " + value);
            }
        }

        public float Sensitivity
        {
            get
            {
                return sensitivity.Value;
            }
            set
            {
                if (value.Approx(Sensitivity)) return;

                PlayerPrefs.SetFloat(SensitivityKey, value);
                PlayerPrefs.Save();
                sensitivity.Value = value;
                Debug.Log("Setting: " + "Sensitivity" + " set to " + value);
            }
        }

        public bool SoundEnabled
        {
            get
            {
                return soundEnabled.Value;
            }
            set
            {
                if (SoundEnabled == value) return;

                PlayerPrefs.SetInt(SoundEnabledKey, value ? 1 : 0);
                PlayerPrefs.Save();
                soundEnabled.Value = value;
                Debug.Log("Setting: " + "Sound" + " set to " + value);
            }
        }

        public bool MusicEnabled
        {
            get
            {
                return musicEnabled.Value;
            }
            set
            {
                if (MusicEnabled == value) return;

                PlayerPrefs.SetInt(MusicEnabledKey, value ? 1 : 0);
                PlayerPrefs.Save();
                musicEnabled.Value = value;
                Debug.Log("Setting: " + "Music" + " set to " + value);
            }
        }

        public enum ThisOrThat
        {
            This,
            That
        }
    }
}