using UnityEngine;

namespace Inectio.Lite
{
    public class InectioBootStrap : MonoBehaviour
    {
        private InectioContext context;
        
		private void Awake()
		{
            context = new InectioContext();
		}

        private void OnDestroy()
        {
            context.OnRemove();
        }
    }
}
