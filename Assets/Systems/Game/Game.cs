using Assets.Scripts.Utils;
using Assets.Systems.EnemyMovementSystem;
using System;
using System.Collections.Generic;
using Assets.Systems.DamagingSystem;
using Assets.Systems.DumbKISystem;
using Assets.Systems.RoomRotationSystem;
using Assets.Systems.EnemyDeathSystem;
using Assets.Systems.EnemySpawnerSystem;
using Assets.Systems.GameCommands;
using Assets.Systems.HealthSystem;
using Assets.Systems.UISystem;
using UnityEngine;

namespace Assets.Systems.Game
{
    public class Game : MonoBehaviour, IGameSystem
    {
        private readonly Dictionary<Type, List<IGameSystem>> _systemToComponentMapper = new Dictionary<Type, List<IGameSystem>>();
        private readonly List<IGameSystem> _gameSystems = new List<IGameSystem>();

        public int Priority { get { return -1; } }
        public List<Type> SystemComponents { get { return null; } }

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

        private void Awake()
        {
            IoC.RegisterSingleton(this);

            #region System Registration

            RegisterSystem(new GameCommandsSystem());
            RegisterSystem(new HealthManager());
            RegisterSystem(new DamageSystem());
            RegisterSystem(new EnemyDeath());
            RegisterSystem(new EnemyMovement());
            RegisterSystem(new DumbKi());
            RegisterSystem(new RoomRotator());
            RegisterSystem(new EnemySpawner());
            RegisterSystem(new UserInteractionSystem());

            #endregion System Registration

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
                foreach (var componentType in system.SystemComponents)
                {
                    MapSystemToComponent(system, componentType);
                }

                system.Init();
            }
        }

        private void MapSystemToComponent(IGameSystem system, Type componentType)
        {
            if (!_systemToComponentMapper.ContainsKey(componentType))
            {
                _systemToComponentMapper.Add(componentType, new List<IGameSystem>());
            }
            _systemToComponentMapper[componentType].Add(system);
        }
    }
}