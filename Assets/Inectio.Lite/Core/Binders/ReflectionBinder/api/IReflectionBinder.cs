using System;

namespace Iniectio.Lite
{
    public interface IReflectionBinder
    {
        IBinding Map<TKey, TValue>();
        IBinding Map<TKey>();
        IBinding Map(Type key, object value);
        IBinding Map(Type type);
        IBinding GetBinding(Type key);
        IBinding GetBinding(Type key, string name);
        void ResolveBinding(IBinding binding);
        ReflectedItems Get(Type type);
        void OnRemove();
		void OnRemove(Type type);
	}
}
