using System;
using System.Collections.Generic;
using Assets.Scripts.Utils;

namespace Assets.Systems.Game
{
    public class Game : IGameSystem
    {
        private readonly Dictionary<Type, List<IGameSystem>> _systemToComponentMapper = new Dictionary<Type, List<IGameSystem>>();
        private readonly List<IGameSystem> _gameSystems = new List<IGameSystem>();

        public int Priority { get { return -1; } }
        public List<IGameComponent> SystemComponents { get { return null; } }

        public void Init()
        {
            MapAllSystemsComponents();
        }

        public void RegisterComponent(IGameComponent component)
        {
            List<IGameSystem> systemsToRegisterTo;
            if (!_systemToComponentMapper.TryGetValue(component.GetType(), out systemsToRegisterTo)) return;

            foreach (var system in systemsToRegisterTo)
            {
                system.RegisterComponent(component);
            }
        }

        private void Start()
        {
            IoC.RegisterSingleton(this);

            Init();
        }

        private void RegisterSystem(IGameSystem system)
        {
            _gameSystems.Add(system);
        }

        private void MapAllSystemsComponents()
        {
            _gameSystems.Sort((system, gameSystem) => system.Priority - gameSystem.Priority);

            foreach (var system in _gameSystems)
            {
                foreach (var component in system.SystemComponents)
                {
                    MapSystemToComponent(system, component);
                }
            }
        }

        private void MapSystemToComponent(IGameSystem system, IGameComponent component)
        {
            if (!_systemToComponentMapper.ContainsKey(component.GetType()))
            {
                _systemToComponentMapper.Add(component.GetType(), new List<IGameSystem>());
            }
            _systemToComponentMapper[component.GetType()].Add(system);
        }
    }
}