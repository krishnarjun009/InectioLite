using System;

namespace Inectio.Lite
{
    public interface ICommand
    {

    }

    public class Command : ICommand
    {        
        public virtual void Execute()
        {

        }
    }

    public class Command<T> : ICommand
    {
        public virtual void Execute(T type)
        {
        }
    }

    public class Command<T, U> : ICommand
    {
        public virtual void Execute(T type1, U type2)
        {

        }
    }

    public class Command<T, U, W> : ICommand
    {
        public virtual void Execute(T type1, U type2, W type3)
        {

        }
    }

    public class Command<T, U, W, X> : ICommand
    {
        public virtual void Execute(T type1, U type2, W type3, X type4)
        {

        }
    }
}
