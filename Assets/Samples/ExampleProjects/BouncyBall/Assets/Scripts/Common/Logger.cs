using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.golivegames.common
{
    public sealed class Logger
    {
        public static void Print(string msg)
        {
#if UNITY_EDITOR
            Debug.Log(msg);
#endif
        }
    }
}
