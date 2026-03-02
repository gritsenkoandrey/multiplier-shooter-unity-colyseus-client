using UnityEngine;

namespace Game
{
    public sealed class HealthView : MonoBehaviour
    {
        [SerializeField] private RectTransform _fill;
        [SerializeField] private float _defaultWidth;

        public void UpdateHealth(int max, int current)
        {
            float ratio = (float) current / max;
            
            _fill.sizeDelta = new (_defaultWidth * ratio, _fill.sizeDelta.y);
        }
    }
}