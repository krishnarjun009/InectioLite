using com.input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace com.bonucyballs
{
    public class BallController : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 3f;

        [SerializeField]
        private float jumpHeight;

        private Rigidbody2D rigidbody;

        private bool leftInput;
        private bool rightInput;
        private bool isGrounded;
        private bool isHighJumpApplied;
        private float screenCenterX;
        private float startJumpHeight;
        private Vector3 startPosition;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            // save the horizontal center of the screen
            screenCenterX = Screen.width * 0.5f;
            startPosition = transform.position;
            startJumpHeight = jumpHeight;
        }

        void OnDisable()
        {
            StandardInput.LeftHandler += StandardInput_LeftHandler;
            StandardInput.RightHandler += StandardInput_RightHandler;
        }

        void OnEnable()
        {
            StandardInput.LeftHandler += StandardInput_LeftHandler;
            StandardInput.RightHandler += StandardInput_RightHandler;
        }

        private void StandardInput_RightHandler()
        {
            leftInput = false;
            rightInput = true;
        }

        private void StandardInput_LeftHandler()
        {
            leftInput = true;
            rightInput = false;
        }

        void Update()
        {
            // if there are any touches currently
            if (Input.touchCount > 0)
            {
                // get the first one
                Touch firstTouch = Input.GetTouch(0);

                // if it began this frame
                if (firstTouch.phase == TouchPhase.Began)
                {
                    if (firstTouch.position.x > screenCenterX)
                    {
                        // if the touch position is to the right of center
                        // move right
                        rightInput = true;
                    }
                    else if (firstTouch.position.x < screenCenterX)
                    {
                        // if the touch position is to the left of center
                        // move left
                        leftInput = true;
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if (isGrounded)
            {
                isGrounded = false;
                Jump();
            }

            if(leftInput)
            {
                leftInput = false;
                Move(-1);
            }

            if(rightInput)
            {
                rightInput = false;
                Move(1);
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
                GameEvents.DispatchLevelCompleted();
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
