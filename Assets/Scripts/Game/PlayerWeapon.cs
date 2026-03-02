using UnityEngine;

namespace Game
{
    public sealed class PlayerWeapon : Weapon
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _fireRate;
        [SerializeField] private int _damage;
        
        private float _lastFireTime;
        
        public bool TryShoot(out ShootInfo info)
        {
            if (Time.time - _lastFireTime < _fireRate)
            {
                info = default;
                return false;
            }
            
            Vector3 position = _spawnPoint.position;
            Vector3 velocity = _spawnPoint.forward * _bulletSpeed;
            
            Bullet bullet = Instantiate(_bulletPrefab, position, _spawnPoint.rotation);
            
            bullet.Initialize(velocity, _damage);
            
            _lastFireTime = Time.time;
            
            OnShoot.Invoke();
            
            info = new ()
            {
                pX = position.x,
                pY = position.y,
                pZ = position.z,
                dX = velocity.x,
                dY = velocity.y,
                dZ = velocity.z,
            };
            
            return true;
        }
    }
}