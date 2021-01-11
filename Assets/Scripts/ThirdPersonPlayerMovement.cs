using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerMovement : MonoBehaviour
{
    [SerializeField] enum movementType {Player, Vehicle}

    public CharacterController controller;
    public Transform cam;

    [SerializeField] private float speed = 6f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float jumpSpeed = 8f;

    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 5f;

    Vector3 moveDir;
    Vector3 direction;
    bool jumpBool = false;
    public float turnSmoothTime = 0.1f;
    float jumpVal;
    public bool inJump = false;
    float turnSmoothVelocity;

    void Start()
    {
        Cursor.visible = false; //Hide mouse cursor.
        Debug.Log(Physics.gravity);
    }

    //Detect inputs in Update.
    void Update()
    {
        //Detect horizontal direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        //Detect vertical direction;
        jumpBool = Input.GetButton("Jump");
    }
    
    //Handle the movement/inputs.
    void FixedUpdate()
    {
        //Check to see if the player is inputting a move direction.
        if (direction.magnitude >= 0.1f)
        {
            //Make player move in camera direction.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Make player face the camera direction when moving.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = moveDir.normalized * speed * Time.deltaTime;

        }
        else
        {
            moveDir = new Vector3(0f, 0f, 0f);
        }

        //Apply jump calculation.
        moveDir.y = jumpLogic();

        //PERFORM CONTROLLER MOVE
        controller.Move(moveDir);
    }

    //Logic which determines player's jumping state.
    float jumpLogic()
    {
        if (controller.isGrounded && jumpBool && inJump == false)    //Player is full jumping.
        {
            jumpVal = jumpSpeed;
            inJump = true;
        }
        else if (!controller.isGrounded && !jumpBool)                //Player is bunny-hopping.
        {
            jumpVal -= gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        else if (controller.isGrounded)                                             //Player is on the ground.
        {
            jumpVal = 0f;

            //Prevent the player from jumping again 
            //until they release the jump button.
            if(!jumpBool)
            { 
                inJump = false;
            }
        }
        else                                                                        //Player is falling.
        {
            jumpVal -= gravity * (fallMultiplier - 1) * Time.deltaTime;
        }

        return jumpVal;
    }

}