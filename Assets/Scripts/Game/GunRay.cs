using UnityEngine;

namespace Game
{
    public sealed class GunRay : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _rayOrigin;
        [SerializeField] private Transform _rayTarget;
        [SerializeField] private float _pointSize;
        
        private Transform _camera;

        private void Awake()
        {
            _camera = Camera.main.transform;
        }

        private void Update()
        {
            Ray ray = new (_rayOrigin.position, _rayOrigin.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 50f, _layerMask, QueryTriggerInteraction.Ignore))
            {
                _rayOrigin.localScale = new (1f, 1f, hit.distance);
                _rayTarget.position = hit.point;
                
                float distance = Vector3.Distance(_camera.position, hit.point);
                
                _rayTarget.localScale = Vector3.one * distance * _pointSize;
            }
        }
    }
}