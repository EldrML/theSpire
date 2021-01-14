using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonVehicleMovement : MonoBehaviour
{
    public float baseHeight = 0.2f;
    public float hoverHeight = 2f;
    public float terrainHeight;
    public bool inRange = false;
    public Vector3 vehiclePosition;
    public Vector3 initPosition;
    public ThirdPersonPlayerMovement playerScript;
    public GameObject turnHandlingObject;
    private RaycastHit hit;
    public Transform raycastPoint;
    private float rotationAmount;

    private float speed = 20.0f;
    private Vector3 forwardDirection;


    // // Start is called before the first frame update
    void Start()
    {
        terrainHeight = Terrain.activeTerrain.SampleHeight(vehiclePosition);
        Debug.Log(terrainHeight);
        initPosition = new Vector3(transform.position.x, terrainHeight + baseHeight, transform.position.z);
        transform.position = initPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate to align with the terrain
        Physics.Raycast(raycastPoint.position, Vector3.down, out hit);
        //Physics.BoxCast(raycastPoint.position, new Vector3(1f,0f,1f), Vector3.down, out hit);
        Debug.Log("Use 4 different raycasts on the corners: https://answers.unity.com/questions/168097/orient-vehicle-to-ground-normal.html");
        transform.up -= (transform.up - hit.normal) * 0.1f;


        if (playerScript.playerLocked || (!playerScript.playerLocked && inRange)) //Player is riding or within range
        {
            //Keep shark at a specific height above the ground
            vehiclePosition = transform.position;
            terrainHeight = Terrain.activeTerrain.SampleHeight(vehiclePosition);
            transform.position = new Vector3(vehiclePosition.x, terrainHeight + hoverHeight, vehiclePosition.z);

            if (playerScript.playerLocked)
            {
                // Rotate with input
                rotationAmount = Input.GetAxis("Horizontal") * 120.0f;
                rotationAmount *= Time.deltaTime;
                turnHandlingObject.transform.Rotate(0.0f, rotationAmount, 0.0f);

                
                forwardDirection = turnHandlingObject.transform.forward;
                transform.position += Input.GetAxisRaw("Vertical") * forwardDirection * Time.deltaTime * speed;
            }


        }
        else //Player is not riding and not in range
        {
            vehiclePosition = transform.position;
            terrainHeight = Terrain.activeTerrain.SampleHeight(vehiclePosition);
            transform.position = new Vector3(vehiclePosition.x, terrainHeight + baseHeight, vehiclePosition.z);
        }
    }

    //When in range of the Shark
    void OnTriggerEnter()
    {
        inRange = true;
        // initPosition = new Vector3(transform.position.x, terrainHeight + baseHeight, transform.position.z);
        // vehiclePosition = transform.position;

        // terrainHeight = Terrain.activeTerrain.SampleHeight(vehiclePosition);
        // transform.position = new Vector3(vehiclePosition.x, terrainHeight + hoverHeight, vehiclePosition.z);
    }

    //When out of range of the Shark
    void OnTriggerExit()
    {
        inRange = false;
        // transform.position = initPosition;
    }
}
