using TMPro;
using UnityEngine;

namespace Game
{
    public sealed class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private int _enemyScore = 0;
        private int _playerScore = 0;

        public void SetEnemyScore(int value)
        {
            _playerScore = value;
            
            UpdateText();
        }
        
        public void SetPlayerScore(int value)
        {
            _enemyScore = value;
            
            UpdateText();
        }

        private void UpdateText()
        {
            _text.text = $"{_playerScore}:{_enemyScore}";
        }
    }
}