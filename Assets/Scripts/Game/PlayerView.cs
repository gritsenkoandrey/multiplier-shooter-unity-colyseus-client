using UnityEngine;

namespace Game
{
    public sealed class PlayerView : Character
    {
        [SerializeField] private CheckJump _checkJump;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _head;
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _minHeadAngle = -90f;
        [SerializeField] private float _maxHeadAngle = 90f;
        [SerializeField] private float _jumDelay = 0.2f;

        private float _inputH;
        private float _inputV;
        private float _rotateY;
        private float _currentRotateX;
        private float _jumpTime;

        private void Start()
        {
            Transform camera = Camera.main.transform;
            camera.parent = _cameraPoint;
            camera.localPosition = Vector3.zero;
            camera.localRotation = Quaternion.identity;
        }

        private void FixedUpdate()
        {
            Move();
            RotateY();
        }

        public void SetInput(float horizontal, float vertical, float rotateY)
        {
            _inputH = horizontal;
            _inputV = vertical;
            _rotateY += rotateY;
        }

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
        {
            position = transform.position;
            rotateY = transform.rotation.eulerAngles.y;
            rotateX = _head.localEulerAngles.x;
            velocity = _rigidbody.linearVelocity;
        }

        public void RotateX(float value)
        {
            _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
            _head.localEulerAngles = new (_currentRotateX, 0f, 0f);
        }

        public void Jump()
        {
            if (_checkJump.IsJumping)
            {
                return;
            }
            
            if (Time.time - _jumpTime < _jumDelay)
            {
                return;
            }
            
            _jumpTime = Time.time;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }

        private void Move()
        {
            Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * Speed;
            velocity.y = _rigidbody.linearVelocity.y;
            Velocity = velocity;
            _rigidbody.linearVelocity = Velocity;
        }
        
        private void RotateY()
        {
            _rigidbody.angularVelocity = new (0f, _rotateY, 0f);
            _rotateY = 0f;
        }
    }
}