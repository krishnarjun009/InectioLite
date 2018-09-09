using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using com.input;

namespace com.bonucyballs
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Button playBtn;

        [SerializeField]
        private Button nextLevelBtn;

        [SerializeField]
        private Button replayBtn;

        [SerializeField]
        private Button homeBtn;

        [SerializeField]
        private Text levelNumberText;

        [SerializeField]
        private Text levelCompletedText;

        [SerializeField]
        private GameObject mainMenuPnl;

        [SerializeField]
        private GameObject gameOverPnl;

        private GameObject currentLevelInstance;

        bool isReplay = false;

        private void OnEnable()
        {
            playBtn.onClick.AddListener(OnPlayButtonClicked);
            homeBtn.onClick.AddListener(OnHomeButtonClicked);
            replayBtn.onClick.AddListener(OnReplayButtonClicked);
            nextLevelBtn.onClick.AddListener(OnNextLevelButtonClicked);
            GameEvents.levelCompletedHandler += GameEvents_levelCompletedHandler;
        }

        private void OnDisable()
        {
            playBtn.onClick.RemoveListener(OnPlayButtonClicked);
            homeBtn.onClick.RemoveListener(OnHomeButtonClicked);
            replayBtn.onClick.RemoveListener(OnReplayButtonClicked);
            nextLevelBtn.onClick.RemoveListener(OnNextLevelButtonClicked);
            GameEvents.levelCompletedHandler -= GameEvents_levelCompletedHandler;
        }

        private void OnHomeButtonClicked()
        {
            gameOverPnl.SetActive(false);
            mainMenuPnl.SetActive(true);
            Time.timeScale = 1;
        }

        private void OnReplayButtonClicked()
        {
            isReplay = true;
            Destroy(currentLevelInstance);
            currentLevelInstance = null;

            GameObject go = Resources.Load<GameObject>("Prefabs/Levels/Level-" + (GameData.currentLevel-1));

            if (go == null)
            {
                go = Resources.Load<GameObject>("Prefabs/Levels/Level-" + UnityEngine.Random.Range(1, GameData.currentLevel - 1));
            }

            levelNumberText.text = "LEVEL: " + (GameData.currentLevel-1).ToString();

            currentLevelInstance = Instantiate(go);
            gameOverPnl.SetActive(false);
            Time.timeScale = 1;
        }

        private void OnNextLevelButtonClicked()
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;

            if (GameData.currentLevel <= 47)
            {
                
                PlayerPrefs.SetInt("currentLevel", GameData.currentLevel);
            }

            GameObject go = Resources.Load<GameObject>("Prefabs/Levels/Level-" + GameData.currentLevel);

            if (go == null)
            {
                go = Resources.Load<GameObject>("Prefabs/Levels/Level-" + UnityEngine.Random.Range(1, GameData.currentLevel - 1));
            }

            levelNumberText.text = "LEVEL: " + GameData.currentLevel.ToString();

            currentLevelInstance = Instantiate(go);
            gameOverPnl.SetActive(false);
            Time.timeScale = 1;
        }

        private void GameEvents_levelCompletedHandler()
        {
            gameOverPnl.SetActive(true);
            levelCompletedText.text = "LEVEL "+GameData.currentLevel.ToString()+" COMPLETED";

            if (isReplay)
            {
                isReplay = false;
                return;
            }

            GameData.currentLevel += 1;
        }

        private void LoadBanner()
        {

        }

        private void OnPlayButtonClicked()
        {
            if (currentLevelInstance != null)
            {
                DestroyImmediate(currentLevelInstance);
                currentLevelInstance = null;
            }
            currentLevelInstance = Instantiate(Resources.Load<GameObject>("Prefabs/Levels/Level-" + GameData.currentLevel));

            levelNumberText.text = "LEVEL: "+GameData.currentLevel.ToString();
            mainMenuPnl.SetActive(false);
            Time.timeScale = 1;
        }

        private void OnLeftButtonClicked()
        {
            StandardInput.DispatchLeftInput();
        }

        private void OnRightButtonClicked()
        {
            StandardInput.DispatchRightInput();
        }
    }
}
