using Inectio.Lite;
using UnityEngine;

namespace com.bonucyballs.inectio
{
    public class MainMenuScreen : View
    {
        [Inject] private OnPlayClickSignal onPlayClickSignal { get; set; }
        [Inject] private GameStartNotifierSignal gameStartNotifierSignal;
        [Inject] private IGameData gameData;

        public void OnPlayClick()
        {
            Debug.Log("Play clicked " + gameData.Level);
            gameStartNotifierSignal.Dispatch(gameData.Level);
            onPlayClickSignal.Dispatch();
            gameObject.SetActive(false);
        }
    }
}
