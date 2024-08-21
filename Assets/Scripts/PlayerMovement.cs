using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerGrab playerGrab;
    [SerializeField] Vector2 input;
    [SerializeField] Vector3 moveDirection;
    CharacterController characterController;
    float verticalInput;
    float horizontalInput;
    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float jumpHeight;
    [Header("Speed Setting")]
    [SerializeField] float currentMoveSpeed;
    [SerializeField] float runSpeed = 8;
    [SerializeField] float walkSpeed = 5;
    [Range(0.02f,1.0f)]
    [SerializeField] float accelerationDuration = 0.12f;
    [Range(0.02f, 1.0f)]
    [SerializeField] float stopDuration = 0.08f;
    [Range(0.02f, 1.0f)]
    [SerializeField] float decelerationDuration = 0.04f;
    [SerializeField] float fallSpeed;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float verticalMove;
    [SerializeField] float horizontalMove;

    [Header("Ground Check")]
    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDistance = 0.5f;
    [SerializeField] float groundCheckRadius = 0.5f;

    [Header("Head Check")]
    
    [SerializeField] LayerMask headHitLayer;
    [SerializeField] float headCheckRadius = 0.5f;
    [SerializeField] float headCheckDistance = 0.5f;

    private void Awake()
    {
        playerGrab = GetComponent<PlayerGrab>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        currentMoveSpeed = walkSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - transform.up * groundCheckDistance, groundCheckRadius);

        Gizmos.DrawWireSphere(transform.position + transform.up * headCheckDistance, headCheckRadius);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager.instance.LevelComplete();
        }
    }
    bool IsGrounded()
    {

        if (Physics.SphereCast(transform.position, groundCheckRadius, -transform.up, out RaycastHit hit, groundCheckDistance, groundLayer))
        {
            if (!isGrounded)
            {
                isGrounded = true;
            }
            if (playerGrab.currentHoveredObject != null && playerGrab.currentHoveredObject == hit.collider.gameObject)
            {
                playerGrab.GrabObject(false);
            }
            if (isGrounded)
            {
                fallSpeed = Mathf.MoveTowards(fallSpeed, 0, gravity * Time.deltaTime);
            }
            return true;

        }




        Ray _headRay = new Ray(transform.position, transform.up);
        if (Physics.SphereCast(_headRay, headCheckRadius, headCheckDistance, headHitLayer))
        {
            if (fallSpeed > 0)
            {
                fallSpeed = 0;
            }
        }
        fallSpeed -= gravity * Time.deltaTime;
        isGrounded = false;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded();
        GetInputs();
        MovePlayer();
    }

    private void FixedUpdate()
    {
        CalculateSpeed();
    }
    private void MovePlayer()
    {
        moveDirection = transform.forward * verticalMove + transform.right * horizontalMove;
        moveDirection.y = fallSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void GetInputs()
    {
        if (GameManager.instance.canMove)
        {
            verticalInput = Input.GetAxisRaw("Vertical");
            horizontalInput = Input.GetAxisRaw("Horizontal");
            input = new Vector2(verticalInput, horizontalInput).normalized;

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                currentMoveSpeed = runSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                currentMoveSpeed = walkSpeed;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                //jumpHeight = gravity
                fallSpeed = jumpSpeed;
            }
        }
        else
        {
            input = Vector2.zero;
        }
        
    }

    private void CalculateSpeed()
    {
        //Vertical
        if (verticalInput != 0)
        {
            if (verticalInput * verticalMove > 0)
            {
                verticalMove = Mathf.MoveTowards(verticalMove, input.x * currentMoveSpeed, (Time.fixedDeltaTime / accelerationDuration) * currentMoveSpeed);
            }
            else
            {
                verticalMove = Mathf.MoveTowards(verticalMove, input.x * currentMoveSpeed, (Time.fixedDeltaTime / decelerationDuration) * currentMoveSpeed);
            }
        }
        else
        {
            verticalMove = Mathf.MoveTowards(verticalMove, 0, (Time.fixedDeltaTime / stopDuration) * currentMoveSpeed);
        }
        //Horizontal
        if (horizontalInput != 0)
        {
            if (horizontalInput * horizontalMove > 0)
            {
                horizontalMove = Mathf.MoveTowards(horizontalMove, input.y * currentMoveSpeed, (Time.fixedDeltaTime / accelerationDuration) * currentMoveSpeed);
            }
            else
            {
                horizontalMove = Mathf.MoveTowards(horizontalMove, input.y * currentMoveSpeed, (Time.fixedDeltaTime / decelerationDuration) * currentMoveSpeed);
            }
        }
        else
        {
            horizontalMove = Mathf.MoveTowards(horizontalMove, 0, (Time.fixedDeltaTime / stopDuration) * currentMoveSpeed);
        }


    }
}
