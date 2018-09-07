
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
		}
	}

    public class SampleData
    {
        public void Print()
        {
            UnityEngine.Debug.Log("Test Working");
        }
    }

    public class TestSignal : Signal
    {
        
    }
}
