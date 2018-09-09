using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.bonucyballs
{
    static public class GameEvents
    {
        static public event Action levelCompletedHandler;

        static public void DispatchLevelCompleted()
        {
            if(levelCompletedHandler != null)
            {
                levelCompletedHandler();
            }
        }
    }
}
