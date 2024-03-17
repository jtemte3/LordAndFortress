using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Player Movement")]
	public float speedWalking = 2.0f;
	public float speedRunning = 5.0f;
	public float speedJump = 750.0f;
	public float speedRotation = 2.0f;
	[Header("Jetpack Mode")]
	public bool enableJetpack = true;
	[Header("Player Spawning")]
	public Camera cam;
	public GameObject playerBody;
	public float spawnHeight = .5f;
	[Header("Ground Detection")]
	public Transform PlayerBase;
	public bool canJump = false;
	public float groundCheckDistance = 1.0f;
	public bool onSlope = false;
	public Vector3 slopeMovementDirection;
	public float slopeMovementBoost = 0.01f;

	public void Start()
	{
		//initialize gravity based on jetpack state
		if (enableJetpack.Equals(true))
		{
			playerBody.GetComponent<Rigidbody>().useGravity = false;
		}
		else
		{
			playerBody.GetComponent<Rigidbody>().useGravity = true;
		}

	}

	// Update is called once per frame
	void Update()
	{
		//Creating a local speed variable that can change
		float speed;


		//Check for jetpack settings
		if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.X))
		{
			if (enableJetpack.Equals(false))
			{
				enableJetpack = true;
				playerBody.GetComponent<Rigidbody>().useGravity = false;
			}
			else
			{
				enableJetpack = false;
				playerBody.GetComponent<Rigidbody>().useGravity = true;
			}

		}

		//Check to sprint
		if (Input.GetKey(KeyCode.LeftShift))
		{
			speed = speedRunning * Time.deltaTime;
		}
		else
		{
			speed = speedWalking * Time.deltaTime;
		}

		//Check for moving forwards
		if (Input.GetKey(KeyCode.W))
		{
			if (onSlope)
            {
				transform.Translate(slopeMovementDirection * (speed + slopeMovementBoost));
			}
            else
            {
				transform.Translate(0, 0, speed);
			}
			
		}
		//Check for moving backwards
		if (Input.GetKey(KeyCode.S))
		{
			if (onSlope)
			{
				transform.Translate(slopeMovementDirection * -(speed + slopeMovementBoost));
			}
			else
			{
				transform.Translate(0, 0, -speed);
			}
		}
		//Check for moving left
		if (Input.GetKey(KeyCode.A))
		{
			transform.Translate(-speed, 0, 0);
		}
		//Check for moving right
		if (Input.GetKey(KeyCode.D))
		{
			transform.Translate(speed, 0, 0);
		}

		//check if jetpack is off
		if (enableJetpack == false)
		{
			RaycastHit hit;
			// This determines if the player is touching the ground or a surface underneath them
			canJump = Physics.Raycast(PlayerBase.position, PlayerBase.TransformDirection(Vector3.down),out hit, groundCheckDistance);

			//Check if the player is on a slope
			if (hit.normal != Vector3.up)
            {
				onSlope = true;
				slopeMovementDirection = Vector3.ProjectOnPlane(Vector3.forward, hit.normal);
            }
            else
            {
				onSlope = false;
            }

			//Check for jumping
			if (canJump.Equals(true) && Input.GetKeyDown(KeyCode.Space))
			{
				playerBody.GetComponent<Rigidbody>().AddForce(0, speedJump, 0, ForceMode.Impulse);
				canJump = false;
			}
		}
		else
		{
			playerBody.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
			//Check for moving up
			if (Input.GetKey(KeyCode.Space))
			{
				transform.Translate(0, speed, 0);
			}
			//Check for moving down
			if (Input.GetKey(KeyCode.C))
			{
				transform.Translate(0, -speed, 0);
			}
		}

		//For camera controls
		//Get the horizontal movement of the mouse to rotate the character from side to side
		float horizontal = speedRotation * Input.GetAxis("Mouse X");
		//Get the vertical movement of the mouse to rotate the camera up and down
		float vertical = speedRotation * Input.GetAxis("Mouse Y");

		//Set the character to move left and right based off the horizontal variable
		transform.Rotate(0, horizontal, 0);
		//Set the camera to move up and down based off the vertical variable. This is not inverted(to invert make it positive)
		cam.transform.Rotate(-vertical, 0, 0);

	}
}
