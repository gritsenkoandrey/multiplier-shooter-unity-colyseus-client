using System.Collections;
using UnityEngine;

namespace Game
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _lifetime = 5f;
        
        public void Initialize(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity;
            
            StartCoroutine(DelayDestroy());
        }

        private void OnCollisionEnter(Collision other)
        {
            Destroy();
        }

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSecondsRealtime(_lifetime);
            
            Destroy();
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}