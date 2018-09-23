using System;
using Iniectio.Lite;
using UnityEngine;

namespace com.bonucyballs.Iniectio
{
    public class SaveGameDataCommand : Command
    {
        [Inject("gamedata")] private IGameData gameData;

		public override void Execute()
		{
            UnityEngine.Debug.Log("Saving Game Data");
            gameData.SaveCurrentLevel();
		}
	}
}
