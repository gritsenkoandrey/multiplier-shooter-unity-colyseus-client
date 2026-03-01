using UnityEngine;

namespace Game
{
    public sealed class CheckJump : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private float _coyoteTime = 0.15f;
        [SerializeField] private LayerMask _groundLayer;

        private float _jumpTimer = 0f;
        
        public bool IsJumping { get; private set; }

        private void Update()
        {
            if (Physics.CheckSphere(transform.position, _radius, _groundLayer))
            {
                IsJumping = false;
                _jumpTimer = 0f;
            }
            else
            {
                _jumpTimer += Time.deltaTime;
                
                if (_jumpTimer > _coyoteTime)
                {
                    IsJumping = true;
                }
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
  #endif
    }
}