﻿using Iniectio.Lite;
using UnityEngine;

namespace com.bonucyballs.Iniectio
{
    public class GameManager : View
    {
        [Inject] private GameStartNotifierSignal gameStartNotifierSignal { get; set; }
        [Inject] private SaveGameDataSignal saveGameDataSignal { get; set; }
        [Inject] IGameData gameData { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            //gameStartNotifierSignal.AddListener()
        }

        private GameObject currentLevelInstance;

        [Listen(typeof(OnNextLevelClickSignal))]
        private void OnNextLevelHanlder()
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
            currentLevelInstance = GetLevel(gameData.Level);
            gameStartNotifierSignal.Dispatch(gameData.Level);
        }

        [Listen(typeof(OnReloadLevelClickSignal))]
        private void OnReloadLevelHanlder()
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
            currentLevelInstance = GetLevel(gameData.Level - 1);
            gameStartNotifierSignal.Dispatch(gameData.Level);
        }

        [Listen(typeof(OnPlayClickSignal))]
        private void OnPlayClick()
        {
            if (currentLevelInstance != null)
            {
                DestroyImmediate(currentLevelInstance);
                currentLevelInstance = null;
            }
            currentLevelInstance = GetLevel(gameData.Level);
            gameStartNotifierSignal.Dispatch(gameData.Level);
            Time.timeScale = 1;
        }

        [Listen(typeof(LevelCompletedSignal))]
        private void OnLevelCompleted()
        {
            saveGameDataSignal.Dispatch();
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }

        private GameObject GetLevel(int level)
        {
            GameObject go = Resources.Load<GameObject>("Prefabs/Levels/Level-" + (gameData.Level - 1));

            if (go == null)
            {
                go = Resources.Load<GameObject>("Prefabs/Levels/Level-" + UnityEngine.Random.Range(1, gameData.Level - 1));
            }

            return Instantiate(go);
        }
    }
}
