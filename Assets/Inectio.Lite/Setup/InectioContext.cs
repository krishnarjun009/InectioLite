using com.bonucyballs.Iniectio;

namespace Iniectio.Lite
{
    public class InectioContext : RootContext
    {
        public InectioContext() : base()
        {
            
        }
        
		public override void mapBindings()
		{
            injectionBinder.Map<SampleData>();
            injectionBinder.Map<TestSignal>();
            injectionBinder.Map<JumpInputSignal>();
            injectionBinder.Map<OnPlayerDiedSignal>();
            injectionBinder.Map<ISample, SampleData>();
            //injectionBinder.Map<commandsignal>();
            //injectionBinder.Map<int>();

            commandBinder.Map<TestSignal, TestCommand>();
            commandBinder.Map<commandsignal, genericcommand>().Pooled();
            commandBinder.Map<commandsignal2, genericcommand>().Pooled();
            //commandBinder.Map<commandsignal, genericcommand1>().ToName("another").Pooled();
            //commandBinder.Map<commandsignal1, genericcommand>().Pooled();

            //Bouncy Ball Inections...
            injectionBinder.Map<LevelCompletedSignal>();
            injectionBinder.Map<LevelFailedSignal>();
            injectionBinder.Map<OnPlayClickSignal>();
            injectionBinder.Map<OnHomeClickSignal>();
            injectionBinder.Map<OnNextLevelClickSignal>();
            injectionBinder.Map<OnReloadLevelClickSignal>();
            injectionBinder.Map<BallInputSignal>();
            injectionBinder.Map<GameStartNotifierSignal>();
            injectionBinder.Map<IGameData, GameData>();
             
            commandBinder.Map<SaveGameDataSignal, SaveGameDataCommand>();
        }
	}

    public interface ISample
    {

    }

    public class SampleData : ISample
    {
        [Inject] private TestSignal testSignal { get; set; }
        public void Print()
        {
            //UnityEngine.Debug.Log("Calling from sample data print method-------");
            testSignal.Dispatch();
        }
    }

   

    public class commandsignal : Signal<int, int, string, int> { }
    public class commandsignal1 : Signal<int, string> { }
    public class commandsignal2 : Signal<int, int, string, int> { }
    public class genericcommand : Command<int, int, string, int>
    {
        [Inject] private TestSignal testSignal { get; set; }

		public override void Execute(int type1, int type2, string type3, int type4)
		{
            //UnityEngine.Debug.Log("Command - type3 data: " + type3);
            testSignal.Dispatch();
		}
	}

    public class genericcommand1 : Command<int, string>
    {
        public override void Execute(int data, string str)
        {
            UnityEngine.Debug.Log("Generic command one");
        }
    }

    public class TestCommand : Command
    {
		public override void Execute()
		{
            //UnityEngine.Debug.Log("Command is working");
		}
	}
}
