using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    public float groundDrag;
    [SerializeField]
    private float swimDrag;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float jumpCooldown;
    [SerializeField]
    private float airMultiplier;
    private bool readyToJump = true;
    [Header("GroundCheck")]
    [SerializeField]
    private float playerHeight;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float minHeight;
    private bool grounded;
    [SerializeField]
    private Transform orientation;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
    }


    private void Update()
    {
       
        grounded = Physics.Raycast(transform.position,Vector3.down, playerHeight * 0.5f + 0.2f);

        MyInput();
        SpeedControl();

        //Swim
        if (transform.position.y <= minHeight && !grounded)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            rigidbody.freezeRotation = true;
            rigidbody.drag = swimDrag;
            Debug.Log("swim");
        }
        //Run
        else if(grounded)
        {
            rigidbody.drag = groundDrag;
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.freezeRotation = true;
            Debug.Log("grounded");
        }
        //Fly
        else if(transform.position.y>=minHeight && !grounded)
        {
            rigidbody.drag = 0;
            rigidbody.freezeRotation = true;
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetAxisRaw("Jump") > 0 && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
        {
            rigidbody.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded)
        {
            rigidbody.AddForce(moveDirection.normalized * moveSpeed * 10f*airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rigidbody.velocity = new Vector3(limitedVel.x, rigidbody.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y velocity 
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
