using Iniectio.Lite;
using UnityEngine;
using UnityEngine.UI;

namespace com.bonucyballs.Iniectio
{
    public class GamePlayScreen : View
    {
        [SerializeField] private Text levelText;
        [SerializeField] private GameObject mainPanel;

        [Listen(typeof(GameStartNotifierSignal))]
        private void OnGameStart(int level)
        {
            //Debug.Log("Game Started");
            levelText.text = string.Concat("Level: " + level);
            mainPanel.SetActive(true);
        }

        [Listen(typeof(LevelCompletedSignal))]
        private void OnLevelCompleted()
        {
            mainPanel.SetActive(false);
        }

        [Listen(typeof(LevelFailedSignal))]
        private void OnLEvelFailed()
        {
            mainPanel.SetActive(false);
        }
    }
}
