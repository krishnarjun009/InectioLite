using System;
using Inectio.Lite;
using UnityEngine;
using UnityEngine.UI;

namespace com.bonucyballs.inectio
{
    public class GameOverScreen : View
    {
        [Inject] private LevelCompletedSignal levelCompletedSignal { get; set; }
        [Inject] private OnNextLevelClickSignal nextLevelClickSignal { get; set; }

        [SerializeField] private Text levelText;

        public override void OnRegister()
        {
            levelCompletedSignal.AddListener(OnGameOverHandler);
        }

        public override void OnRemove()
        {
            levelCompletedSignal.RemoveListener(OnGameOverHandler);
        }

        private void OnGameOverHandler()
        {
            gameObject.SetActive(true);
        }

        [Listen(typeof(GameStartNotifierSignal))]
        private void OnGameStart(int level)
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        public void OnReloadClick()
        {

        }

        public void OnHomeClick()
        {

        }

        public void OnNextLevelClick()
        {
            nextLevelClickSignal.Dispatch();
        }
    }
}
