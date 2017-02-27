using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour{
    // Still not used, work in progress...
	public enum PlayerMovementStatus : byte{
        STAND = 0,
        IDLE = 1,
		WALKING = 2
	}

    private PlayerMovementStatus movement_status;   // Work in progress...
    private Animator _animator;    // Used for animations.

    private Vector3 movement_vector;    // Used for movements. Idle: (0,0,0).

    public float movement_speed = 5.0f; // Used for movements. Affects whole character speed.
    private float rotation_angle;
    
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
        rotation_angle = Vector3.Angle(Vector3.forward, transform.forward);
        _animator=this.GetComponent<Animator>(); // Gets the animator from the attached GO.
        isMovementCoroutineRunning=false;        // Sets running=false to enable movement coroutine calls.
    }

    /// Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
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
        if(movement_vector != Vector3.zero){ // If movement vector isn't null...
            rotation_angle=Mathf.Atan2 (movement_vector.x, movement_vector.z) * Mathf.Rad2Deg; // Calculate angle rotation for player (in degrees).
            if(!isMovementCoroutineRunning){
                StartCoroutine(executeMovement()); // Delegate movement to 'async' coroutine function to save performance.
            }
        }
    }

    /*
        executeMovement is a coroutine called from FixedUpdate method to manage efficiently character movement.
        It relies on movement_vector value (set from Update() function).
    */
    private bool isMovementCoroutineRunning; // This value estabilishes if coroutine is already called.
    IEnumerator executeMovement(){
        // Coroutine started running, so the movement vector is != (0,0,0). Let's warn that this coroutine can't be runned twice.
        isMovementCoroutineRunning=true;

        // Notify animator that the character started moving.
        _animator.SetBool("isMoving", true);

        // Until the movement vector is not (0,0,0), execute the movement each FixedUpdate frame.
        while(movement_vector != Vector3.zero){
            transform.position+=((movement_vector=movement_vector*Time.deltaTime));  // Make movement and reduce movement_vector on deltaTime.
            transform.eulerAngles=new Vector3(transform.eulerAngles.x, rotation_angle, transform.eulerAngles.z); // Make angle-based rotation.
            
            yield return CoRoutineWaitBuilder.getInstance().m_WaitForFixedUpdate;   // Wait for next frame elaboration.
        }
        
        // Notify animator that the character stopped moving.
        _animator.SetBool("isMoving", false);

        // Character speed is zero. Coroutine is already going to die. Let's warn that this coroutine can be runned again.
        isMovementCoroutineRunning=false;
    }


}