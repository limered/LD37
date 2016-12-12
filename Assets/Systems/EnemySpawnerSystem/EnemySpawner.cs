using Assets.Systems.EnemySpawnerSystem.Components;
using System;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Systems.EnemySpawnerSystem
{
    internal class EnemySpawner : IGameSystem
    {
        public int Priority { get { return 12; } }

        public List<Type> SystemComponents
        {
            get
            {
                return new List<Type>
                {
                    typeof(SpawnerComponent)
                };
            }
        }

        public void Init()
        {
        }

        public void RegisterComponent(IGameComponent component)
        {
            var comp = component as SpawnerComponent;
            if (comp)
            {
                comp.FixedUpdateAsObservable()
                    .Where(_ => comp.IsActive)
                    .Where(_ => CheckForSpawn(comp))
                    .Subscribe(_ => SpawnEnemies(comp))
                    .AddTo(comp);
            }
        }

        private bool CheckForSpawn(SpawnerComponent comp)
        {
            comp.SpawnTimer += Time.fixedDeltaTime;
            return comp.SpawnTimer > comp.SpawnInterval;
        }

        private void SpawnEnemies(SpawnerComponent comp)
        {
            comp.SpawnTimer = 0;
            var coll = comp.GetComponent<BoxCollider>();
            var areaToSpawn = new Bounds(comp.transform.position, coll.size);

            var enemiesToSpawn = (comp.InFiniteSpawn) ? comp.SpawnCountPerTick : Math.Min(comp.SpawnCountPerTick, comp.MaxEnemiesToSpawn);
            if(!comp.InFiniteSpawn)
                comp.MaxEnemiesToSpawn -= enemiesToSpawn;

            Observable.Range(0, enemiesToSpawn)
                .Subscribe(_ => SpawnEnemy(comp.EnemyToSpawn, comp.Parent, areaToSpawn));

            if(comp.MaxEnemiesToSpawn == 0 && !comp.InFiniteSpawn)
                GameObject.Destroy(comp.gameObject);
        }

        private void SpawnEnemy(GameObject enemy, GameObject parent, Bounds area)
        {
            var pos = new Vector3().RandomVector(area.min, area.max);
            var axis = Random.value < 0.5f ? new Vector3(1, 0, 0) : new Vector3(0, 0, 1);
            var angle = ((int)(Random.value*720)/90) * 90;
            var enemyObject = GameObject.Instantiate(enemy, pos, Quaternion.AngleAxis(angle, axis));
            if (parent)
            {
                enemyObject.transform.parent = parent.transform;
                enemyObject.transform.localRotation = Quaternion.AngleAxis(angle, axis);
            }
        }
    }
}