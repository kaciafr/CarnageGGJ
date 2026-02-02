using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Mouvement")]
        public float movementSpeed = 5f;
        public float groundDrag = 5f;

        [Header("Saut / Sol")]
        public float playerHeight = 2f;
        public LayerMask whatIsGround;
        public float jumpForce = 10f;
        public float jumpCoolDown = 0.25f;
        public float airMultiplier = 0.4f;
        public KeyCode jumpKey = KeyCode.Space;

        [Header("Références")]
        public Transform orientation;
        

        private float horizontalInput;
        private float verticalInput;

        private bool grounded;
        private bool readyToJump = true;

        private Vector3 moveDirection = Vector3.zero;
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody manquant sur le joueur !");
                enabled = false;
                return;
            }

            rb.freezeRotation = true; 
        }

        private void Update()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            MovementInput();
            SpeedControl();

            rb.linearDamping = grounded ? groundDrag : 0f;
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MovementInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCoolDown);
            }
        }

        private void MovePlayer()
        {
            if (orientation == null) return; 

            moveDirection = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

            float forceMultiplier = 10f;
            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * (movementSpeed * forceMultiplier), ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * (movementSpeed * forceMultiplier * airMultiplier), ForceMode.Force);
            }
        }

        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            if (flatVel.magnitude > movementSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * movementSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            readyToJump = true;
        }
    }
}
