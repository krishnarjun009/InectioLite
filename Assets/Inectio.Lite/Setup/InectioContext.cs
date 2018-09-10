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
}
