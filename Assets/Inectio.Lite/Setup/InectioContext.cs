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
		}
	}

    public interface ISample
    {

    }

    public class SampleData : ISample
    {
        public void Print()
        {
            UnityEngine.Debug.Log("Test Working");
        }
    }

    public class commandsignal : Signal<int> { }
    public class genericcommand : Command<int>
    {
		public override void Execute(int data)
		{
            UnityEngine.Debug.Log("Command is working as generic " + data);
		}
	}

    public class TestCommand : Command
    {
		public override void Execute()
		{
            UnityEngine.Debug.Log("Command is working");
		}
	}
}
