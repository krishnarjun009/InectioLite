using System;
using Iniectio.Lite;
using UnityEngine;

namespace com.bonucyballs.Iniectio
{
    public class InputDirectionCommand : Command<InputDirection>
    {
        [Inject] private InputDirectionSignal inputDirectionSignal { get; set; }

        public override void Execute(InputDirection inputDirection)
		{
            switch(inputDirection)
            {
                case InputDirection.LEFT:
                    inputDirectionSignal.Dispatch(-1);
                    break;
                default:
                    inputDirectionSignal.Dispatch(1);
                    break;
            }
		}
	}
}
