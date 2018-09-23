using Iniectio.Lite;
using UnityEngine;

namespace com.bonucyballs.Iniectio
{
    public class MainMenuScreen : View
    {
        [Inject] private OnPlayClickSignal onPlayClickSignal { get; set; }
        [Inject] private GameStartNotifierSignal gameStartNotifierSignal;
        [Inject] private IGameData gameData;

        public void OnPlayClick()
        {
            Debug.Log("Play clicked " + gameData.Level);
           // gameStartNotifierSignal.Dispatch(gameData.Level);
            onPlayClickSignal.Dispatch();
            gameObject.SetActive(false);
        }

        [Listen(typeof(OnHomeClickSignal))]
        private void OnHomeClickHandler()
        {
            gameObject.SetActive(true);
        }
    }
}
