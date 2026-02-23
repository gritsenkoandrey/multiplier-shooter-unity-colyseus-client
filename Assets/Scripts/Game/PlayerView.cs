using UnityEngine;

namespace Game
{
    public sealed class PlayerView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 2f;

        private float _inputH;
        private float _inputV;

        private void FixedUpdate()
        {
            Move();
        }

        public void SetInput(float horizontal, float vertical)
        {
            _inputH = horizontal;
            _inputV = vertical;
        }

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
        {
            position = transform.position;
            velocity = _rigidbody.linearVelocity;
        }

        private void Move()
        {
            Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
            
            _rigidbody.linearVelocity = velocity;
        }
    }
}