using UnityEngine;

namespace Iniectio.Lite
{
    public class InectioBootStrap : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad = true;
        private InectioContext context;
        
		private void Awake()
		{
            context = new InectioContext();
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(this);
		}

        private void OnDestroy()
        {
            context.OnRemove();
        }
    }
}
