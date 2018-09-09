using Inectio.Lite;
using UnityEngine;

public class InputController : View {

    [Inject] private JumpInputSignal jumpInputSignal;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpInputSignal.Dispatch();
        }
    }
}
