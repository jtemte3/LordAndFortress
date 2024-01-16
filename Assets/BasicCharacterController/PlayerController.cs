using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float spawnHeight = .5f;
	public float speedWalking = 2.0f;
	public float speedRunning = 5.0f;
	public float speedJump = 750.0f;
	public float speedRotation = 2.0f;
	public bool jetpackMode = true;
	public Camera cam;
	public GameObject playerBody;
	public Transform PlayerBase;
	public bool canJump = false;
	float distance = 1.0f;

	public void Start()
	{
		//initialize gravity based on jetpack state
		if (jetpackMode.Equals(true))
		{
			playerBody.GetComponent<Rigidbody>().useGravity = false;
		}
		else
		{
			playerBody.GetComponent<Rigidbody>().useGravity = true;
		}
		//Set Cursor to the middle of the game window
		Cursor.lockState = CursorLockMode.Locked;
		//Set Cursor to not be visible
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		//Creating a local speed variable that can change
		float speed;

		//Check for cursor settings
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SetCursor();
		}

		//Check for jetpack settings
		if (Input.GetKeyDown(KeyCode.X))
		{
			if (jetpackMode.Equals(false))
			{
				jetpackMode = true;
				playerBody.GetComponent<Rigidbody>().useGravity = false;
			}
			else
			{
				jetpackMode = false;
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
			transform.Translate(0, 0, speed);
		}
		//Check for moving backwards
		if (Input.GetKey(KeyCode.S))
		{
			transform.Translate(0, 0, -speed);
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
		if (jetpackMode == false)
		{
			// This determines if the player is touching the ground or a surface underneath them
			canJump = Physics.Raycast(PlayerBase.position, PlayerBase.TransformDirection(Vector3.down), distance);

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

	void SetCursor()
	{

		if (Cursor.lockState == CursorLockMode.Locked)
		{
			//Unlock the Cursor
			Cursor.lockState = CursorLockMode.None;
			//Set Cursor to be visible
			Cursor.visible = true;
		}
		if (Cursor.lockState == CursorLockMode.None)
		{
			//Lock the Cursor
			Cursor.lockState = CursorLockMode.Locked;
			//Set Cursor to not be visible
			Cursor.visible = false;
		}

	}
}
