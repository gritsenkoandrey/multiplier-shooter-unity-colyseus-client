using UnityEngine;

namespace Game
{
    public sealed class CharacterAnimation : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private CheckJump _checkJump;
        [SerializeField] private Animator _animator;
        
        private readonly int _groundedId = Animator.StringToHash("Grounded");
        private readonly int _speedId = Animator.StringToHash("Speed");
        
        private void Update()
        {
            Vector3 localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
            float speed = localVelocity.magnitude / _character.Speed;
            float sign = Mathf.Sign(localVelocity.z);
            speed *= sign;

            _animator.SetFloat(_speedId, speed);
            _animator.SetBool(_groundedId, _checkJump.IsJumping == false);
        }
    }
}