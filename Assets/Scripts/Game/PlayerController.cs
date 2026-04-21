using System.Collections;
using System.Collections.Generic;
using Colyseus.Schema;
using Infrastructure;
using Newtonsoft.Json;
using Settings;
using UnityEngine;

namespace Game
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private PlayerWeapon _playerWeapon;
        [SerializeField] private Health _health;
        [SerializeField] private float _restartDelay = 3f;
        [SerializeField] private float _mouseSensitivity = 2f;

        private bool _isRestart;
        private bool _hideCursor;

        private readonly Dictionary<string, object> _data = new ()
        {
            { "pX", 0f }, { "pY", 0f }, { "pZ", 0f }, 
            { "vX", 0f }, { "vY", 0f }, { "vZ", 0f }, 
            { "rX", 0f }, { "rY", 0f }
        };

        public int MaxHealth => _playerView.MaxHealth;
        public float Speed => _playerView.Speed;
        
        private MultiplierService _multiplierService;
        private ScoreView _scoreView;
        private SpawnPoints _spawnPoints;

        private void Awake()
        {
            _multiplierService = MultiplierService.Instance;
            _hideCursor = true;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _hideCursor = !_hideCursor;
                
                Cursor.lockState = _hideCursor ? CursorLockMode.Locked : CursorLockMode.None;
            }
            
            if (_isRestart)
            {
                return;
            }
            
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            float mouseX = 0f;
            float mouseY = 0f;
            bool isFire = false;

            if (_hideCursor)
            {
                mouseX = Input.GetAxisRaw("Mouse X");
                mouseY = Input.GetAxisRaw("Mouse Y");
                isFire = Input.GetMouseButtonDown(0);
            }
            
            bool isJump = Input.GetKeyDown(KeyCode.Space);
            
            _playerView.SetInput(horizontal, vertical, mouseX * _mouseSensitivity);
            _playerView.RotateX(-mouseY * _mouseSensitivity);

            if (isJump)
            {
                _playerView.Jump();
            }

            if (isFire && _playerWeapon.TryShoot(out ShootInfo info))
            {
                SendShoot(ref info);
            }
            
            SendMove();
        }

        public void Initialize(Player player, StateCallbackStrategy<State> callbacks, HealthView healthView, ScoreView scoreView, SpawnPoints spawnPoints)
        {
            _scoreView = scoreView;
            _spawnPoints = spawnPoints;
            
            _health.SetHealthView(healthView);
            _health.SetMax(player.maxHp);
            _health.SetCurrent(player.curHp);
            
            callbacks.Listen(player, p => p.curHp, (cur, prev) => _health.SetCurrent(cur));
            callbacks.Listen(player, p => p.loss, (loss, prev) => _scoreView.SetPlayerScore(loss));
        }
        
        private void SendMove()
        {
            _playerView.GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY);
            
            _data["pX"] = position.x;
            _data["pY"] = position.y;
            _data["pZ"] = position.z;
            _data["vX"] = velocity.x;
            _data["vY"] = velocity.y;
            _data["vZ"] = velocity.z;
            _data["rX"] = rotateX;
            _data["rY"] = rotateY;

            _multiplierService.SendMessage("move", _data);
        }

        private void SendShoot(ref ShootInfo info)
        {
            info.key = _multiplierService.GetSessionId();
            
            string json = JsonConvert.SerializeObject(info, JsonSettings.Settings);
            
            _multiplierService.SendMessage("shoot", json);
        }
        
        public void Restart(int spawnIndex)
        {
            StartCoroutine(DelayRestart());
            
            Transform spawnPoint = _spawnPoints.GetSpawnPoint(spawnIndex);
           
            _playerView.transform.position = spawnPoint.position;
            _playerView.transform.rotation = spawnPoint.rotation;
            
            _playerView.SetInput(0f, 0f, 0f);
            
            _data["pX"] = spawnPoint.position.x;
            _data["pY"] = spawnPoint.position.y;
            _data["pZ"] = spawnPoint.position.z;
            _data["vX"] = 0;
            _data["vY"] = 0;
            _data["vZ"] = 0;
            _data["rX"] = 0;
            _data["rY"] = spawnPoint.eulerAngles.y;

            _multiplierService.SendMessage("move", _data);
        }
        
        private IEnumerator DelayRestart()
        {
            _isRestart = true;
            yield return new WaitForSecondsRealtime(_restartDelay);
            _isRestart = false;
        }
    }

    [System.Serializable]
    public struct ShootInfo
    {
        public string key;
        public float pX;
        public float pY;
        public float pZ;
        public float dX;
        public float dY;
        public float dZ;
    }
}