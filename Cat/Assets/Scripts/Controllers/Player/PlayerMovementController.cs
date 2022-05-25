using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovementController : MonoBehaviour, ResetableGameObject {

    private enum JUMP_STATE  {
        None,
        Jumped,
        DoubleJumped,
        Flying
    }

    public GameObject CurrentGameObject {
        get { return gameObject; }
    }

    Rigidbody rb;
    
    Quaternion cameraRotation;
    float cameraY;
    Vector3 moveDirection;
    Quaternion mouseRotation;
    
    float mouseX;
    [SerializeField]
    private float jumpHeight = 7.0f;
    [SerializeField]
    private float doubleJumpHeight = 5.0f;
    [SerializeField]
    private float mouseRotationSpeed = 2.0f;
    [SerializeField]
    private float moveSpeed = 5.0f;
    [SerializeField]
    private bool isDoubleJumpEnabled = true;
    private JUMP_STATE jumpState = JUMP_STATE.None;
    Vector3 jumpVector;
    Vector3 doubleJumpVector;
    int countAction = 0;
    int layerMask = 1 << Config.Layers.PLAYER;
    [SerializeField]
    float speedDowned = 0.5f;
    float height;

    void Start() {
        layerMask = ~layerMask;
        rb = GetComponent<Rigidbody>();
        jumpVector = new Vector3(0, jumpHeight, 0);
        Cursor.lockState = CursorLockMode.Locked;
        doubleJumpVector  = new Vector3(0, doubleJumpHeight, 0);
        height = GetComponent<MeshFilter>().mesh.bounds.extents.y;
    }


    void Update () {
        ProcessInputs();
    }

    void FixedUpdate() {
        Move();
        Rotate();
    }
    
    bool IsGrounded () {
        RaycastHit hit;

        Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, height + 0.01f, layerMask);

        return hit.collider != null;
    }

    void ProcessInputs () {
        float moveX = Input.GetAxisRaw("Vertical");
        float moveZ = Input.GetAxisRaw("Horizontal");

        mouseX += Input.GetAxisRaw("Mouse X");

        mouseRotation = Quaternion.Euler(0, mouseX * mouseRotationSpeed, 0);

        bool isGrounded = IsGrounded();

        Vector3 forwardVector = transform.forward * moveX;
        Vector3 sideVector = transform.right * moveZ;
        if(Input.GetKeyDown(KeyCode.J))
        {
            Action();
        }
        if (isGrounded) {
            moveDirection = forwardVector + sideVector;
        }
        else
        {
            moveDirection = (forwardVector + sideVector) * speedDowned;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Action()
    {
        if(countAction == 0)
        {
            Debug.Log("мяу");
        }
        if(countAction == 1)
        {
            Debug.Log("укус");
        }
        if(countAction == 2)
        {
            Debug.Log("когти");
        }
        countAction++;
        if(countAction > 2)
        {
            countAction = 0;
        }
    }

    void Move () {
        Vector3 movePosition = rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(movePosition);
    }

    void Jump () {

        bool isGrounded = IsGrounded();

        if(isGrounded)
        {
            rb.AddForce(jumpVector + moveDirection, ForceMode.Impulse);
            jumpState = JUMP_STATE.Jumped;
        }
        else if(isDoubleJumpEnabled & jumpState == JUMP_STATE.Jumped)
        {
            rb.AddForce(doubleJumpVector, ForceMode.Impulse);
            jumpState = JUMP_STATE.DoubleJumped;
        }
    }

    void Rotate () {
        transform.rotation = mouseRotation;
    }
}