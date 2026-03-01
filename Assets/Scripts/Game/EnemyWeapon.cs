using UnityEngine;

namespace Game
{
    public sealed class EnemyWeapon : Weapon
    {
        public void Shoot(Vector3 position, Vector3 velocity)
        {
            Bullet bullet = Instantiate(_bulletPrefab, position, Quaternion.identity);
            
            bullet.Initialize(velocity);
            
            OnShoot.Invoke();
        }
    }
}