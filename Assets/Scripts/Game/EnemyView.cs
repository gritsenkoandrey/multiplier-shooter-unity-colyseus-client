using UnityEngine;

namespace Game
{
    public sealed class EnemyView : Character
    {
        [SerializeField] private Transform _head;
        
        private const float EPSILON = 0.1f;
        
        private Vector3 _position;
        private float _velocityMagnitude;

        private void Awake()
        {
            _position = transform.position;
        }

        private void Update()
        {
            if (_velocityMagnitude > EPSILON)
            {
                transform.position = Vector3.MoveTowards(transform.position, _position, _velocityMagnitude * Time.deltaTime);
            }
            else
            {
                transform.position = _position;
            }
        }
        
        public void SetPosition(in Vector3 position, in Vector3 velocity, in float averageInterval)
        {
            _position = position + velocity * averageInterval;
            _velocityMagnitude = velocity.magnitude;
            
            Velocity = velocity;
        }

        public void SetRotateX(float value)
        {
            _head.localEulerAngles = new (value, 0f, 0f);
        }
        
        public void SetRotateY(float value)
        {
            transform.localEulerAngles = new (0f, value, 0f);
        }
        
        public void SetSpeed(float value)
        {
            Speed = value;
        }
    }
}