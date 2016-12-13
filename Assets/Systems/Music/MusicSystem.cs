using System;
using System.Collections.Generic;
using Assets.Systems.GameControl;
using Assets.Systems.GameControl.Components;
using UniRx;

namespace Assets.Systems.Music
{
    public class MusicSystem : IGameSystem
    {
        private MusicHelper _helper;
        private bool _isMuted = false;
        public int Priority { get { return 22; } }
        public List<Type> SystemComponents { get { return new List<Type> { typeof(MusicHelper), typeof(GameControlHelper) }; } }

        public void Init()
        {
            MessageBroker.Default.Receive<ChangeMusicArgs>().Subscribe(ToggleMuteMusic);
        }

        private void ToggleMuteMusic(ChangeMusicArgs changeMusicArgs)
        {
            if (_isMuted)
            {
                _helper.Song.Play();
            }
            else
            {
                _helper.Song.Stop();
            }
            _isMuted = !_isMuted;
        }

        public void RegisterComponent(IGameComponent component)
        {
            RegisterComponent(component as MusicHelper);
            RegisterComponent(component as GameControlHelper);
        }

        public void RegisterComponent(GameControlHelper helper)
        {
            if (helper)
            {
                helper
                    .GameMode
                    .Where(mode => mode == GameMode.Running)
                    .Subscribe(_ => StartMusic())
                    .AddTo(helper);
                helper
                    .GameMode
                    .Where(mode => mode == GameMode.End || mode == GameMode.StartSequence)
                    .Subscribe(_ => StopMusic())
                    .AddTo(helper);
            }
        }

        private void StopMusic()
        {
            _helper.Song.Stop();
        }

        private void StartMusic()
        {
            if (_isMuted) return;
            _helper.Song.Play();
        }

        private void RegisterComponent(MusicHelper helper)
        {
            if (helper) _helper = helper;
        }
    }
}