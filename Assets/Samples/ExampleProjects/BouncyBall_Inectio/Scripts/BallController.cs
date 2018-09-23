using com.input;
using Iniectio.Lite;
using UnityEngine;

namespace com.bonucyballs.Iniectio
{
    public enum InputDirection
    {
        LEFT,
        RIGHT,
        NONE
    }

    public class BallController : View
    {
        private LevelCompletedSignal levelCompletedSignal;

        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float jumpHeight;

        private Rigidbody2D rigidbody;
        private bool leftInput;
        private bool rightInput;
        private bool isGrounded;
        private bool isInputApplied = false;
        private bool isHighJumpApplied;
        private float startJumpHeight;
        private int direction;
        private Vector3 startPosition;

        [Inject]
        private void InjectObjes(LevelCompletedSignal level)
        {
            levelCompletedSignal = level;
        }

		protected override void Start()
		{
            base.Start();
            rigidbody = GetComponent<Rigidbody2D>();
            // save the horizontal center of the screen
            startPosition = transform.position;
            startJumpHeight = jumpHeight;
		}

        [Listen(typeof(InputDirectionSignal))]
        private void OnBallInputHandler(int direction)
        {
            Debug.Log("Direction " + direction);
            this.isInputApplied = true;
            this.direction = direction;
        }

        void FixedUpdate()
        {
            if (isGrounded)
            {
                isGrounded = false;
                Jump();
            }

            if(isInputApplied)
            {
                isInputApplied = false;
                Move(direction);
            }
        }

        protected virtual void Move(int direction)
        {
            Vector2 velocity = rigidbody.velocity;
            velocity.x = moveSpeed * direction;
            rigidbody.velocity = velocity;
        }

        protected virtual void Jump()
        {
            if (rigidbody != null)
            {
                //isGrounded = false;
                Vector2 velocity = rigidbody.velocity;
                velocity.y = jumpHeight;
                rigidbody.velocity = velocity;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Reloader") || collision.collider.CompareTag("die"))
            {
                // reset ball postion
                transform.position = startPosition;
            }

            else if (!isHighJumpApplied && collision.collider.CompareTag("Highjumper"))
            {
                isHighJumpApplied = true;
                isGrounded = true;
                // reset ball postion
                jumpHeight *= 2;
            }

            else
            {
                transform.SetParent(collision.collider.gameObject.transform);
                jumpHeight = startJumpHeight;
                isHighJumpApplied = false;
                isGrounded = true;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Levelend"))
            {
                // level end
                levelCompletedSignal.Dispatch();
                collider.gameObject.SetActive(false);

            }

            else if (collider.CompareTag("Highjumper"))
            {
                // reset ball postion
                Debug.Log("high jump");
                jumpHeight *= 2;
            }
        }
    }
}
