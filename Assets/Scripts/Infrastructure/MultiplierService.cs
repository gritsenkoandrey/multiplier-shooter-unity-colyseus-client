using System.Collections.Generic;
using Colyseus;
using Colyseus.Schema;
using Game;
using Newtonsoft.Json;
using UnityEngine;

namespace Infrastructure
{
    public sealed class MultiplierService : Manager<MultiplierService>
    {
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private EnemyController _enemyPrefab;
        [SerializeField] private HUD _hudPrefab;
        [SerializeField] private SpawnPoints _spawnPoints; 

        private Room<State> _room;
        private StateCallbackStrategy<State> _callbacks;
        private readonly Dictionary<string, EnemyController> _enemies = new ();
        private HUD _hud;
        
        protected override void Awake()
        {
            base.Awake();
            
            _hud = Instantiate(_hudPrefab);
            
            Instance.InitializeClient();
            
            Connect();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _room.Leave();
        }

        private async void Connect()
        {
            Transform spawnPoint = _spawnPoints.GetRandomSpawnPoint();
            
            Vector3 pos = spawnPoint.position;
            
            Dictionary<string, object> options = new ()
            {
                {"speed", _playerPrefab.Speed},
                {"maxHp", _playerPrefab.MaxHealth},
                {"curHp", _playerPrefab.MaxHealth},
                {"pX", pos.x},
                {"pY", pos.y},
                {"pZ", pos.z},
                {"rY", spawnPoint.eulerAngles.y},
                {"sPoints", _spawnPoints.Length},
            };
            
            _room = await Instance.client.JoinOrCreate<State>("state_handler", options);
            
            _callbacks = Callbacks.Get(_room);
            _callbacks.OnAdd(state => state.players, Add);
            _callbacks.OnRemove(state => state.players, Remove);
            _room.OnMessage<string>("shoot_action", ApplyShoot);
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
            Quaternion rotation = Quaternion.Euler(0f, player.rY, 0f);

            PlayerController view = Instantiate(_playerPrefab, position, rotation);
            
            view.Initialize(player, _callbacks, _hud.HealthView, _hud.ScoreView, _spawnPoints);
            
            _room.OnMessage<int>("restart", view.Restart);
        }

        private void SpawnEnemy(string key, Player player)
        {
            Vector3 position = new (player.pX, player.pY, player.pZ);

            EnemyController view = Instantiate(_enemyPrefab, position, Quaternion.identity);

            view.Initialize(key, player, _hud.ScoreView, _callbacks);
            
            _enemies.Add(key, view);
        }
        
        private void ApplyShoot(string jsonShootInfo)
        {
            ShootInfo shootInfo = JsonConvert.DeserializeObject<ShootInfo>(jsonShootInfo);

            if (_enemies.TryGetValue(shootInfo.key, out EnemyController enemy))
            {
                enemy.Shoot(shootInfo);
            }
            else
            {
                Debug.LogError("Enemy Not Found");
            }
        }

        public void SendMessage(string key, Dictionary<string, object> data) => _room.Send(key, data);

        public void SendMessage(string key, string data) => _room.Send(key, data);

        public string GetSessionId() => _room.SessionId;
    }
}