using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
    public Rigidbody rb;
    public Camera camera;
    
    Quaternion cameraRotation;
    float cameraY;
    Vector3 moveDirection;

    bool isJump = false;
    bool isOnTheGround = false;
    Quaternion mouseRotation;

    float mouseX;
    float mouseY;
    public float jumpHeight = 7.0f;
    public float mouseRotationSpeed = 2.0f;
    public float moveSpeed = 5.0f;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == 6) {
            isJump = false;
            isOnTheGround = true;
        }
    }

    void Update () {
        ProcessInputs();
    }

    void FixedUpdate() {
        Move();    
        Jump();
        Rotate();
        RotateCamera();
    }


    void ProcessInputs () {
        float moveX = Input.GetAxisRaw("Vertical");
        float moveZ = Input.GetAxisRaw("Horizontal");

        mouseX += Input.GetAxisRaw("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y") * 10;
        mouseY = Mathf.Clamp(mouseY, 0, 45);

        mouseRotation = Quaternion.Euler(0, mouseX * mouseRotationSpeed, 0);

        cameraY = Mathf.Lerp(1, 5, mouseY / 45);

        Vector3 forwardVector = transform.forward * moveX;
        Vector3 sideVector = transform.right * moveZ;

        moveDirection = forwardVector + sideVector;

        if (Input.GetKeyDown("space") && !isJump) {
            isJump = true;
        }
    }

    void Move () {
        Vector3 movePosition = rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime);

        rb.MovePosition(movePosition);
    }



    void Jump () {
        if (isJump && isOnTheGround) {
            isOnTheGround = false;
            Vector3 jumpVector = new Vector3(0, jumpHeight, 0);

            rb.AddForce(jumpVector, ForceMode.Impulse);
        }
    }

    void Rotate () {
        transform.rotation = mouseRotation;
    }

    void RotateCamera () {
        Vector3 cameraEuler = camera.transform.rotation.eulerAngles;
        cameraEuler.x = mouseY;
        camera.transform.rotation = Quaternion.Euler(cameraEuler);
        camera.transform.position = new Vector3(camera.transform.position.x, cameraY, camera.transform.position.z);
    }
}