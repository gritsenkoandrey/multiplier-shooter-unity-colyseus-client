using System.Collections.Generic;
using Infrastructure;
using UnityEngine;

namespace Game
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerView _playerView;

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            _playerView.SetInput(horizontal, vertical);
            
            SendMove();
        }
        
        private void SendMove()
        {
            _playerView.GetMoveInfo(out Vector3 position, out Vector3 velocity);

            Dictionary<string, object> data = new ()
            {
                { "pX", position.x },
                { "pY", position.y },
                { "pZ", position.z },
                { "vX", velocity.x },
                { "vY", velocity.y },
                { "vZ", velocity.z },
            };

            MultiplierService.Instance.SendMessage("move", data);
        }
    }
}