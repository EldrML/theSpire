using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToParent : MonoBehaviour
{
    //public Transform parentTransform;
    public CharacterController childController;
    public ThirdPersonPlayerMovement childMoveScript;

    [SerializeField] bool locked;
    [SerializeField] bool inRange;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("c") && !locked && inRange)
        {
            locked = true;

            childMoveScript.enabled = false;
            childController.enabled = false;

            transform.SetParent(this.transform);
            childController.transform.position = this.transform.position + 2f * transform.up - 3f * transform.forward;
            childController.transform.rotation = this.transform.rotation;
        }
        else if (Input.GetKeyDown("c") && locked)
        {
            locked = false;
            transform.SetParent(null);
            childMoveScript.enabled = true;
            childController.enabled = true;
        }

    }
    void OnTriggerEnter()
    {
        inRange = true;
    }

    void OnTriggerExit()
    {
        inRange = false;
    }
}
