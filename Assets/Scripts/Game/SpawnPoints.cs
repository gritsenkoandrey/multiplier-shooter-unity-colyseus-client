using UnityEngine;

namespace Game
{
    public sealed class SpawnPoints : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        
        public int Length => _spawnPoints.Length;
        
        public Transform GetRandomSpawnPoint() => _spawnPoints[Random.Range(0, Length)];
        
        public Transform GetSpawnPoint(int index) => _spawnPoints[index];
    }
}