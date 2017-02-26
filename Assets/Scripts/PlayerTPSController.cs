using UnityEngine;
using System.Collections;

public class PlayerTPSController : MonoBehaviour {
	public enum PlayerMovementStatus : byte{
		IDLE = 0,
		WALKING = 1,
		RUNNING = 2,
		WALKING_BACK = 3,
		WALKING_RIGHT = 4,
		WALKING_LEFT = 5
	}

	//This variable indicates how is the current state of character.
	private PlayerMovementStatus status;

	//This variable indicates if the player is aiming or not.
	private bool is_aiming; 

	//Define the turning speed.
	private float rotation_speed = 4.0f;


	private float horizontal;

	private Animator animator;
	private Vector3 position_player; // centroDaTela
	private CursorLockMode mouseTrancado;


	public bool block_controls;

	//Get the camera properties.
	public Camera camera_player; 

	void Start ()
	{
		position_player.x = 0.5f;
		position_player.y = 0.5f;
		position_player.z = 0f;
		animator = GetComponent<Animator>();
		animator.SetBool("isIdle", false);
		status = PlayerMovementStatus.IDLE;
		is_aiming = false;
		block_controls = false;
		horizontal = transform.eulerAngles.y;
	}

	void Update ()
	{
		//print("isIdle="+animator.GetBool("isIdle"));
		//FocoRaycast();
		if (block_controls == false)
		{
			/*Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;*/
			Controle();
		}
		moveCharacter();
		//animateCharacter();
		FocoCamera();
	}

	private void animateCharacter()
	{
		//animator.SetInteger("Status", (int)status);
	}

	private void Controle()
	{
		if (Input.GetKeyDown("w"))
		{
			status = PlayerMovementStatus.WALKING;
		}
		if (Input.GetKeyUp("w") && status == PlayerMovementStatus.WALKING)
		{
			status = PlayerMovementStatus.IDLE;
			if (Input.GetKey("s")) { status = PlayerMovementStatus.WALKING_BACK; }
			if (Input.GetKey("a")) { status = PlayerMovementStatus.WALKING_LEFT; }
			if (Input.GetKey("d")) { status = PlayerMovementStatus.WALKING_RIGHT; }
		}
		if (Input.GetKeyUp("w") && status == PlayerMovementStatus.RUNNING)
		{
			status = PlayerMovementStatus.IDLE;
		}

		if (Input.GetKeyDown(KeyCode.LeftShift) && status == PlayerMovementStatus.WALKING)
		{
			status = PlayerMovementStatus.RUNNING;
			if (is_aiming == true){
				is_aiming = false;
			}
		}
		if (Input.GetKeyUp(KeyCode.LeftShift) && status == PlayerMovementStatus.RUNNING) { status = PlayerMovementStatus.WALKING; }

		if (Input.GetKeyDown("s"))
		{
			status = PlayerMovementStatus.WALKING_BACK;
		}
		if (Input.GetKeyUp("s") && status == PlayerMovementStatus.WALKING_BACK)
		{
			status = PlayerMovementStatus.IDLE;
			if (Input.GetKey("a")) { status = PlayerMovementStatus.WALKING_LEFT; }
			if (Input.GetKey("d")) { status = PlayerMovementStatus.WALKING_RIGHT; }
			if (Input.GetKey("w")) { status = PlayerMovementStatus.WALKING; }
		}

		if (Input.GetKeyDown("d"))
		{
			status = PlayerMovementStatus.WALKING_RIGHT;
		}
		if (Input.GetKeyUp("d") && status == PlayerMovementStatus.WALKING_RIGHT)
		{
			status = PlayerMovementStatus.IDLE;
			if (Input.GetKey("s")) { status = PlayerMovementStatus.WALKING_BACK; }
			if (Input.GetKey("a")) { status = PlayerMovementStatus.WALKING_LEFT; }
			if (Input.GetKey("w")) { status = PlayerMovementStatus.WALKING; }

		}

		if (Input.GetKeyDown("a"))
		{
			status = PlayerMovementStatus.WALKING_LEFT;
		}
		if (Input.GetKeyUp("a") && status == PlayerMovementStatus.WALKING_LEFT)
		{
			status = PlayerMovementStatus.IDLE;
			if (Input.GetKey("s")) { status = PlayerMovementStatus.WALKING_BACK; }
			if (Input.GetKey("d")) { status = PlayerMovementStatus.WALKING_RIGHT; }
			if (Input.GetKey("w")) { status = PlayerMovementStatus.WALKING; }
		}

		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			is_aiming = true;
			if (status == PlayerMovementStatus.RUNNING)
			{
				status = PlayerMovementStatus.WALKING;
			}
		}
		if (Input.GetKeyUp(KeyCode.Mouse1)) { is_aiming = false; }
		if(Input.GetKey("i"))
			animator.SetBool("isIdle", true);
		if(Input.GetKey("o"))
			animator.SetBool("isIdle", false);
	}

	private void FocoCamera()
	{
		if (is_aiming && camera_player.fieldOfView > 37)
		{
			camera_player.fieldOfView = camera_player.fieldOfView - 65.0f * Time.deltaTime;
		}
		if (!is_aiming && camera_player.fieldOfView < 60)
		{
			camera_player.fieldOfView = camera_player.fieldOfView + 65.0f * Time.deltaTime;
		}
	}

	/*private void FocoRaycast()
	{
		RaycastHit hitInfo;
		Ray cameraRay = camera_player.ViewportPointToRay(position_player);
	}*/

	private void moveCharacter(){
		var mouseHorizontal = Input.GetAxis("Mouse X");
		horizontal = (horizontal + rotation_speed * mouseHorizontal) % 360f;
		transform.rotation = Quaternion.AngleAxis(horizontal, Vector3.up); // Handles mouse X orientation

		if (status == PlayerMovementStatus.IDLE) 			{ transform.Translate(0, 0, 0); }
		if (status == PlayerMovementStatus.WALKING) 		{ transform.Translate(0, 0, 1.0f * Time.deltaTime); }
		if (status == PlayerMovementStatus.RUNNING) 		{ transform.Translate(0, 0, 5.0f * Time.deltaTime); }
		if (status == PlayerMovementStatus.WALKING_BACK) 	{ transform.Translate(0, 0, -1.0f * Time.deltaTime); }
		if (status == PlayerMovementStatus.WALKING_RIGHT) 	{ transform.Translate(1.0f * Time.deltaTime, 0, 0); }
		if (status == PlayerMovementStatus.WALKING_LEFT) 	{ transform.Translate(-1.0f * Time.deltaTime, 0, 0); }
	}

	public bool Retornais_aiming()
	{
		return is_aiming;
	}

	bool AnimatorIsPlaying(){
		return animator.GetCurrentAnimatorStateInfo(0).length >
			animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}

	bool AnimatorIsPlaying(string stateName){
		return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
	}
}
