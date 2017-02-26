using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour{
    // Still not used, work in progress...
	public enum PlayerMovementStatus : byte{
        IDLE = 0,
		WALKING = 1,
		RUNNING = 2,
		WALKING_BACK = 3,
		WALKING_RIGHT = 4,
		WALKING_LEFT = 5
	}

    private PlayerMovementStatus movement_status;   // Work in progress...
    private Animator _animator;    // For animations.

    private Vector3 movement_vector;    // For movements. Idle: (0,0,0).

    private float horizontal;   // Horizontal rotation for mouse look.
    
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    void Start()
    {
        /*
        // Set player position to the center of the screen.

        position_player.x = 0.5f;
		position_player.y = 0.5f;
		position_player.z = 0f;
        */
        _animator=this.GetComponent<Animator>(); // Gets the animator from the attached GO.
        horizontal = transform.eulerAngles.y;    // Sets rotation from the actual focus-rotation of the player based on Y axis.
        isMovementCoroutineRunning=false;        // Sets running=false to enable movement coroutine calls.
    }

    /// Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        //movement_vector = Vector3.zero;
        if(Input.GetKey("w")){
            movement_vector.z = 5.0f;    // Go up.
        }
        if(Input.GetKey("a")){
            movement_vector.x = -5.0f;    // Go left.
        }
        if(Input.GetKey("s")){
            movement_vector.z = -5.0f;    // Go down.
        }
        if(Input.GetKey("d")){
            movement_vector.x = 5.0f;    // Go right.
        }
        if(Input.GetKey("i")){
            _animator.SetBool("isIdle", false);
        }else if(Input.GetKey("o")){
            _animator.SetBool("isIdle", true);
        }
    }

    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        float mouseHorizontal = Input.GetAxis("Mouse X");
		horizontal = (horizontal + 4f * mouseHorizontal) % 360f;
		transform.rotation = Quaternion.AngleAxis(horizontal, Vector3.up); // Handles mouse X orientation
        
        if(movement_vector!=Vector3.zero && !isMovementCoroutineRunning){
            StartCoroutine(executeMovement()); // Delegate movement to 'async' coroutine function to save performance.
        }
    }

    /*
        executeMovement is a coroutine called from FixedUpdate method to manage efficiently character movement.
        It relies on movement_vector value (set from Update() function).
    */
    private bool isMovementCoroutineRunning; // This value estabilishes if coroutine is already called.
    IEnumerator executeMovement(){
        // Coroutine started running, so the movement vector is != (0,0,0). 'Callback' through setting bool to true.
        isMovementCoroutineRunning=true;

        // Until the movement vector is not (0,0,0), execute the movement each FixedUpdate frame.
        while(movement_vector != Vector3.zero){
            transform.Translate((movement_vector=movement_vector*Time.deltaTime));  // Make movement and reduce movement_vector on deltaTime.
            yield return CoRoutineWaitBuilder.getInstance().m_WaitForFixedUpdate;   // Wait next frame elaboration.
        }

        // Character speed is zero. Coroutine is already going to die. Let's 'callback' through setting bool to false.
        isMovementCoroutineRunning=false;
    }


}