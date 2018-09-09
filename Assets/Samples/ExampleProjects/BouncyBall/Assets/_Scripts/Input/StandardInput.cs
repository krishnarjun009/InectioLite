using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace com.input
{
    public class StandardInput : MonoBehaviour
    {
        public static event Action TapHandler;
        public static event Action ShootHandler;
        public static event Action LeftHandler;
        public static event Action RightHandler;

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (TapHandler != null)
                    TapHandler();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (LeftHandler != null)
                    LeftHandler();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (RightHandler != null)
                    RightHandler();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
            }
#endif
            if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began )
            {
                //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;

                if (TapHandler != null)
                    TapHandler();
            }
        }

        public static void DispatchLeftInput()
        {
            if (LeftHandler != null)
                LeftHandler();
        }

        static public void DispatchRightInput()
        {
            if (RightHandler != null)
                RightHandler();
        }
    }
}
