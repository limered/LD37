using UnityEngine;

namespace Assets.Systems.EnemySpawnerSystem.Components
{
    [RequireComponent(typeof(BoxCollider))]
    public class SpawnerComponent : GameComponent
    {
        public bool IsActive;
        public GameObject Parent;
        public GameObject EnemyToSpawn;

        public int MaxEnemiesToSpawn;
        public int SpawnCountPerTick;
        public float SpawnInterval;
        public float SpawnTimer { get; set; }
    }
}