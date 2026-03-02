using System.Collections;
using UnityEngine;

namespace Game
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _lifetime = 5f;

        private int _damage;
        
        public void Initialize(Vector3 velocity, int damage = 0)
        {
            _rigidbody.linearVelocity = velocity;
            _damage = damage;
            
            StartCoroutine(DelayDestroy());
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.TryGetComponent(out EnemyView enemy))
            {
                enemy.ApplyDamage(_damage);
            }
            
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