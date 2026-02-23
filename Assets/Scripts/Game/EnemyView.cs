using UnityEngine;

namespace Game
{
    public sealed class EnemyView : MonoBehaviour
    {
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
        }
    }
}