using UnityEngine;

namespace Game
{
    public sealed class Health : MonoBehaviour
    {
        [SerializeField] private HealthView _healthView;
        
        private int _max;
        private int _current;
        
        public void SetHealthView(HealthView healthView) => _healthView = healthView;
        
        public void SetMax(int max)
        {
            _max = max;
            UpdateHealth();
        }

        public void SetCurrent(int current)
        {
            _current = current;
            UpdateHealth();
        }

        public void ApplyDamage(int damage)
        {
            _current -= damage;
            UpdateHealth();
        }
        
        private void UpdateHealth() => _healthView.UpdateHealth(_max, _current);
    }
}