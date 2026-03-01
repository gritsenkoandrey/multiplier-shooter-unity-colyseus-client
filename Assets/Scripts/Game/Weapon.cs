using System;
using UnityEngine;

namespace Game
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected Bullet _bulletPrefab;
        
        public Action OnShoot = delegate { };
    }
}