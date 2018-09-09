using Inectio.Lite;
using System;

namespace com.bonucyballs.inectio
{
    public class BallInputSignal : Signal<InputDirection> { }
    public class LevelCompletedSignal : Signal { }
    public class LevelFailedSignal : Signal { }
    public class OnNextLevelClickSignal : Signal { }
    public class OnReloadLevelClickSignal : Signal { }
    public class OnHomeClickSignal : Signal { }
    public class GameStartNotifierSignal : Signal<int> { }
    public class OnPlayClickSignal : Signal { }
}
