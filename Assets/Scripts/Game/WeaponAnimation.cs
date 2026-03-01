using UnityEngine;

namespace Game
{
    public sealed class WeaponAnimation : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Animator _animator;

        private readonly int _shootId = Animator.StringToHash("Shoot");

        private void OnEnable()
        {
            _weapon.OnShoot += Shoot;
        }

        private void OnDisable()
        {
            _weapon.OnShoot -= Shoot;
        }
        
        private void Shoot()
        {
            _animator.SetTrigger(_shootId);
        }
    }
}