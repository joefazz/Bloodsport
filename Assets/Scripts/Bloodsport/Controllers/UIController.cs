using TMPro;
using UnityEngine;

namespace Controllers
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverUI;

        [SerializeField] private TextMeshProUGUI scoreReadout;

        public void InitDependencies(GameplayEventDispatcher gameplayEventDispatcher)
        {
            gameplayEventDispatcher.onEnemyKilled += IncreaseScore;
            gameplayEventDispatcher.onPlayerKilled += GameOver;
        }

        private void GameOver()
        {
            gameOverUI.SetActive(true);
        }

        private void IncreaseScore()
        {
            scoreReadout.text = (int.Parse(scoreReadout.text) + 5).ToString();
        }
    }
}