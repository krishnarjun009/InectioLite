using System;
using UnityEngine;

namespace com.bonucyballs.Iniectio
{
    public interface IGameData
    {
        void SetLevel(int level);
        void SaveCurrentLevel();
        
        int Level { get; }
    }

    public class GameData : IGameData
    {
        private int currentLevel = 1;

        public int Level { get { return currentLevel; } }

        public void SaveCurrentLevel()
        {
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            ++currentLevel;
        }

        public void SetLevel(int level)
        {
            currentLevel = level;
        }
    }
}
