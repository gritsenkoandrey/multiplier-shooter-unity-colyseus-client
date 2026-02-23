using Colyseus.Schema;
using UnityEngine;

namespace Game
{
    public sealed class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyView _enemyView;
        
        private const int INTERVAL_BUFFER_SIZE = 5;
        private readonly float[] _receiveTimeIntervals = new float[INTERVAL_BUFFER_SIZE];
        private int _intervalIndex;
        private float _intervalSum;
        private float _lastReceiveTime;

        public void Initialize(Player player, StateCallbackStrategy<State> callbacks)
        {
            callbacks.OnChange(player, () =>
            {
                SaveReceiveTime(Time.time);
                
                Vector3 newPosition = new (player.pX, player.pY, player.pZ);
                Vector3 newVelocity = new (player.vX, player.vY, player.vZ);
                
                _enemyView.SetPosition(newPosition, newVelocity, GetAverageInterval());
            });
        }

        private void SaveReceiveTime(float time)
        {
            float interval = time - _lastReceiveTime;
            _lastReceiveTime = time;
    
            float oldValue = _receiveTimeIntervals[_intervalIndex];
            _receiveTimeIntervals[_intervalIndex] = interval;
            _intervalSum += interval - oldValue;
    
            _intervalIndex = (_intervalIndex + 1) % INTERVAL_BUFFER_SIZE;
        }
        
        private float GetAverageInterval()
        {
            return _intervalSum / INTERVAL_BUFFER_SIZE;
        }
    }
}