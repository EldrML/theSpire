using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToParent : MonoBehaviour
{
    //public Transform parentTransform;
    public CharacterController childController;
    public ThirdPersonPlayerMovement childMoveScript;
    public ThirdPersonVehicleMovement parentMoveScript;

    [SerializeField] bool inRange;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c") && !childMoveScript.playerLocked && parentMoveScript.inRange)
        {
            childMoveScript.playerLocked = true;    //set the player's state as locked to the vehicle.
            childMoveScript.enabled = false;        //Turn off the player's movescript
            childController.enabled = false;        //Turn off the player's Controller

            transform.SetParent(this.transform);
            //childController.transform.position = this.transform.position + (transform.position.y)* transform.up - 3f * transform.forward;
            //Debug.Log(transform.position);
            //childController.transform.position = this.transform.position + (this.transform.position.y + (1f-0.25f/2f))* transform.up - 3f * transform.forward;
            //childController.transform.rotation = this.transform.rotation;
            
        }
        else if (Input.GetKeyDown("c") && childMoveScript.playerLocked)
        {
            childMoveScript.playerLocked = false;
            transform.SetParent(null);
            childMoveScript.enabled = true;
            childController.enabled = true;
        }

        if(childMoveScript.playerLocked)
        {
            childController.transform.position = this.transform.position + (parentMoveScript.hoverHeight-0.125f)*transform.up - 3f * transform.forward;
            childController.transform.rotation = this.transform.rotation;
        }

    }
    // void OnTriggerEnter()
    // {
    //     inRange = true;
    // }

    // void OnTriggerExit()
    // {
    //     inRange = false;
    // }
}
