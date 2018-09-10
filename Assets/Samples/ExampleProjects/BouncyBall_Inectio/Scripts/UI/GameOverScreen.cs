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
        [Inject] private OnReloadLevelClickSignal reloadLevelClickSignal;
        [Inject] private OnHomeClickSignal onHomeClickSignal;

        [SerializeField] private Text levelText;
        [SerializeField] private GameObject mainPanel;

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
            mainPanel.SetActive(true);
        }

        [Listen(typeof(GameStartNotifierSignal))]
        private void OnGameStart(int level)
        {
            mainPanel.SetActive(false);
        }

        public void OnReloadClick()
        {
            mainPanel.SetActive(false);
            reloadLevelClickSignal.Dispatch();
        }

        public void OnHomeClick()
        {
            mainPanel.SetActive(false);
            onHomeClickSignal.Dispatch();
        }

        public void OnNextLevelClick()
        {
            nextLevelClickSignal.Dispatch();
        }
    }
}
