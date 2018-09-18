using Iniectio.Lite;
using UnityEngine;

public class PlayerController : View {

    public bool canJump = false;
    public float maxJump = 3f;

    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(canJump)
        {
            canJump = false;
            Vector3 velocity = new Vector3(0, maxJump, 0);
            rigidbody.velocity = velocity;
        }
    }

    [Listen(typeof(JumpInputSignal))]
    private void onJumpInput()
    {
        canJump = true;
    }
}
