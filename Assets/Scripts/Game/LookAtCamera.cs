using UnityEngine;

namespace Game
{
    public sealed class LookAtCamera : MonoBehaviour
    {
        private Transform _camera;

        private void Awake()
        {
            _camera = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(_camera);
        }
    }
}