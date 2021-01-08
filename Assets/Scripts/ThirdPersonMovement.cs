using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    [SerializeField] private float speed = 6f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float jumpSpeed = 14f;
    Vector3 moveDir;
    
    public float turnSmoothTime = 0.1f;
    public bool isGrounded;
    float turnSmoothVelocity;

    void Start()
    {
        Cursor.visible = false; //Hide mouse cursor.
    }
    
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //Apply gravity to moveDir.
        if (controller.isGrounded && Input.GetButton("Jump"))
        {
            moveDir.y = jumpSpeed;
        }
        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);

        //Check to see if the player is inputting a move direction.
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;


            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        //Complete the character move.
        // if(moveDir.normalized > 0)
        // {

        // }



    }
    
}
