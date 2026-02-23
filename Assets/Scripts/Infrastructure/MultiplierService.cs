using System.Collections.Generic;
using Colyseus;
using Colyseus.Schema;
using Game;
using UnityEngine;

namespace Infrastructure
{
    public sealed class MultiplierService : Manager<MultiplierService>
    {
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private EnemyController _enemyPrefab;

        private Room<State> _room;
        private StateCallbackStrategy<State> _callbacks;
        private readonly Dictionary<string, EnemyController> _enemies = new ();
        
        protected override void Awake()
        {
            base.Awake();
            
            Instance.InitializeClient();
            
            Connect();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _room.OnStateChange -= OnStateChange;
            _room.Leave();
        }

        private async void Connect()
        {
            _room = await Instance.client.JoinOrCreate<State>("state_handler");
            
            _room.OnStateChange += OnStateChange;
            
            _callbacks = Callbacks.Get(_room);
            _callbacks.OnAdd(state => state.players, Add);
            _callbacks.OnRemove(state => state.players, Remove);
        }
        
        private void OnStateChange(State state, bool isFirstState)
        {
            if (isFirstState) return;
        }
        
        private void Add(string key, Player player)
        {
            if (key == _room.SessionId)
            {
                SpawnPlayer(player);
            }
            else
            {
                SpawnEnemy(key, player);
            }
        }

        private void Remove(string key, Player player)
        {
            if (_enemies.ContainsKey(key))
            {
                Destroy(_enemies[key].gameObject);
                
                _enemies.Remove(key);
            }
        }

        private void SpawnPlayer(Player player)
        {
            Vector3 position = new (player.pX, player.pY, player.pZ);

            PlayerController view = Instantiate(_playerPrefab, position, Quaternion.identity);
        }

        private void SpawnEnemy(string key, Player player)
        {
            Vector3 position = new (player.pX, player.pY, player.pZ);

            EnemyController view = Instantiate(_enemyPrefab, position, Quaternion.identity);

            view.Initialize(player, _callbacks);
            
            _enemies.Add(key, view);
        }

        public void SendMessage(string key, Dictionary<string, object> data)
        {
            _room.Send(key, data);
        }
    }
}