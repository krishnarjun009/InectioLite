using System;

namespace Inectio.Lite
{
    public class Command
    {        
        public virtual void Execute()
        {

        }
    }

    public class Command<T>
    {
        public virtual void Execute(T data)
        {

        }
    }
}
