using System;

namespace Inectio.Lite
{
    public interface ICoreBinder
    {
        IBinding Map<TKey, TValue>();
        IBinding Map<TKey>();
        IBinding Map(Type key, object value);
        IBinding Map(Type type);
        IBinding GetBinding(Type key);
        IBinding GetBinding(Type key, string name);
        void ResolveBinding(IBinding binding);
        void Debug();
    }
}
