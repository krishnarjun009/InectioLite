using Inectio.Lite;
using UnityEngine;

namespace com.bonucyballs.inectio
{
    public class InputController : View
    {
        [Inject] private BallInputSignal ballInputSignal;
        private float screenCenterX;

        protected override void Awake()
        {
            base.Awake();
            screenCenterX = Screen.width * 0.5f;
        }

        private void Update()
        {
            // if there are any touches currently
            if (Input.touchCount > 0)
            {
                // get the first one
                Touch firstTouch = Input.GetTouch(0);

                // if it began this frame
                if (firstTouch.phase == TouchPhase.Began)
                {
                    if (firstTouch.position.x > screenCenterX)
                    {
                        // if the touch position is to the right of center
                        // move right
                        ballInputSignal.Dispatch(InputDirection.RIGHT);
                    }
                    else if (firstTouch.position.x < screenCenterX)
                    {
                        // if the touch position is to the left of center
                        // move left
                        ballInputSignal.Dispatch(InputDirection.LEFT);
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //Debug.Log("LEft clicked");
                ballInputSignal.Dispatch(InputDirection.LEFT);
            }
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                ballInputSignal.Dispatch(InputDirection.RIGHT);
            }
        }
    }
}
