using Inectio.Lite;
using UnityEngine;
using UnityEngine.UI;

namespace com.bonucyballs.inectio
{
    public class GamePlayScreen : View
    {
        [SerializeField] private Text levelText;

        [Listen(typeof(GameStartNotifierSignal))]
        private void OnGameStart(int level)
        {
            //Debug.Log("Game Started");
            levelText.text = string.Concat("Level: " + level);
            gameObject.SetActive(true);
        }

        [Listen(typeof(LevelCompletedSignal))]
        private void OnLevelCompleted()
        {
            gameObject.SetActive(false);
        }

        [Listen(typeof(LevelFailedSignal))]
        private void OnLEvelFailed()
        {
            gameObject.SetActive(false);
        }
    }
}
