using com.bonucyballs.inectio;

namespace Inectio.Lite
{
    public class InectioContext : RootContext
    {
        public InectioContext() : base()
        {
            
        }
        
		public override void MapBindings()
		{
            injectionBinder.Map<SampleData>();
            injectionBinder.Map<TestSignal>();
            injectionBinder.Map<JumpInputSignal>();
            injectionBinder.Map<OnPlayerDiedSignal>();
            injectionBinder.Map<ISample, SampleData>();

            commandBinder.Map<TestSignal, TestCommand>().Pooled();
            commandBinder.Map<commandsignal, genericcommand>().Pooled();
            //commandBinder.Map<commandsignal, genericcommand1>().ToName("another").Pooled();
            commandBinder.Map<commandsignal1, genericcommand>().Pooled();

            //Bouncy Ball Inections...
            //injectionBinder.Map<LevelCompletedSignal>();
            //injectionBinder.Map<LevelFailedSignal>();
            //injectionBinder.Map<OnPlayClickSignal>();
            //injectionBinder.Map<OnHomeClickSignal>();
            //injectionBinder.Map<OnNextLevelClickSignal>();
            //injectionBinder.Map<OnReloadLevelClickSignal>();
            //injectionBinder.Map<BallInputSignal>();
            //injectionBinder.Map<GameStartNotifierSignal>();
            //injectionBinder.Map<IGameData, GameData>();
        }
	}

    public interface ISample
    {

    }

    public class SampleData : ISample
    {
        public void Print()
        {
            //UnityEngine.Debug.Log("Test Working");
        }
    }

    public class commandsignal : Signal<int, string> { }
    public class commandsignal1 : Signal<int, string> { }
    public class genericcommand : Command<int, string>
    {
		public override void Execute(int data, string str)
		{
            UnityEngine.Debug.Log("Command is working as generic " + str);
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
