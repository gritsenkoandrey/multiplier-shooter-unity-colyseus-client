using System.Collections.Generic;
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
        [SerializeField] private float _mouseSensitivity = 2f;

        private readonly Dictionary<string, object> _data = new ()
        {
            { "pX", 0f }, { "pY", 0f }, { "pZ", 0f }, 
            { "vX", 0f }, { "vY", 0f }, { "vZ", 0f }, 
            { "rX", 0f }, { "rY", 0f }
        };
        
        private MultiplierService _multiplierService;

        private void Awake()
        {
            _multiplierService = MultiplierService.Instance;
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");
            
            bool isJump = Input.GetKeyDown(KeyCode.Space);
            bool isFire = Input.GetMouseButtonDown(0);
            
            _playerView.SetInput(horizontal, vertical, mouseX * _mouseSensitivity);
            _playerView.RotateX(-mouseY * _mouseSensitivity);

            if (isJump) _playerView.Jump();

            if (isFire && _playerWeapon.TryShoot(out ShootInfo info))
            {
                SendShoot(ref info);
            }
            
            SendMove();
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
            info.key = _multiplierService.GetKey();
            
            string json = JsonConvert.SerializeObject(info, JsonSettings.Settings);
            
            _multiplierService.SendMessage("shoot", json);
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