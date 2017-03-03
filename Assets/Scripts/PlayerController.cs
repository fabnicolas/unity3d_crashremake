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
    private Rigidbody _rigidbody;   // Used for movements.

    private Vector3 movement_vector;    // Used for movements. Idle: (0,0,0).

    public float movement_speed = 5.0f; // Used for movements. Affects whole character speed.
    private float rotation_angle;


	private float timestamp_last_movement;
    
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
        _rigidbody=this.GetComponent<Rigidbody>();

        // Sets coroutines execution to false in order to enable first calls.
        isExecuteMovementCoroutineRunning=false;
        isProgressiveChangeRotationCoroutineRunning=false;

        // At beginning you should not run idle animation, so we consider player as if he already did a movement.
        timestamp_last_movement=Time.time;
    }

    /// Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if(Input.GetKey("w")){
            movement_vector.z = movement_speed*1.0f;    // Go up.
        }
        if(Input.GetKey("a")){
            movement_vector.x = movement_speed*-1.0f;    // Go left.
        }
        if(Input.GetKey("s")){
            movement_vector.z = movement_speed*-1.0f;    // Go down.
        }
        if(Input.GetKey("d")){
            movement_vector.x = movement_speed*1.0f;    // Go right.
        }
        if(Input.GetKey(KeyCode.Space)){
            _rigidbody.AddForce(Vector3.up*2.0f, ForceMode.Impulse);
        }
    }

    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        if((Time.time - timestamp_last_movement) >= 4){
            if(!isProgressiveChangeRotationCoroutineRunning){
                StartCoroutine(progressiveChangeRotation(180,0.2f));
            }
        }
        if(movement_vector != Vector3.zero){ // If movement vector isn't null...
            rotation_angle=Mathf.Atan2 (movement_vector.x, movement_vector.z) * Mathf.Rad2Deg; // Calculate angle rotation for player (in degrees).
            if(!isExecuteMovementCoroutineRunning){
                StartCoroutine(executeMovement()); // Delegate movement to 'async' coroutine function to save performance.
            }
            timestamp_last_movement = Time.time;
            _animator.SetBool("isIdle", false);
        }
    }

    /*
        executeMovement is a coroutine called from FixedUpdate method to manage efficiently character movement.
        It relies on movement_vector value (set from Update() function).
    */
    private bool isExecuteMovementCoroutineRunning; // This value estabilishes if coroutine is already called.
    IEnumerator executeMovement(){
        // Coroutine started running, so the movement vector is != (0,0,0). Let's warn that this coroutine can't be runned twice.
        isExecuteMovementCoroutineRunning=true;

        // Notify animator that the character started moving.
        _animator.SetBool("isMoving", true);

        // Until the movement vector is not (0,0,0), execute the movement each FixedUpdate frame.
        while(movement_vector != Vector3.zero){
            transform.position+=((movement_vector=movement_vector*Time.deltaTime));  // Make movement and reduce movement_vector on deltaTime.
            this.changeRotation(rotation_angle); // Make angle-based rotation.
            
            yield return CoRoutineWaitBuilder.getInstance().m_WaitForFixedUpdate;   // Wait for next frame elaboration.
        }
        
        // Notify animator that the character stopped moving.
        _animator.SetBool("isMoving", false);

        // Character speed is zero. Coroutine is already going to die. Let's warn that this coroutine can be runned again.
        isExecuteMovementCoroutineRunning=false;
    }

    // Sets the rotation of the player instantly.
    void changeRotation(float rotation_y){
         transform.eulerAngles=new Vector3(transform.eulerAngles.x, rotation_y, transform.eulerAngles.z);
    }

    /*
        progressiveChangeRotation is a coroutine called from FixedUpdate method to manage efficiently character rotation over time.
        It relies on changeRotation function. It executes smooth rotation in a specific time.
     */
    private bool isProgressiveChangeRotationCoroutineRunning;
    IEnumerator progressiveChangeRotation(float rotation_y, float time){
        // Disable extra coroutine calls.
        isProgressiveChangeRotationCoroutineRunning=true;

        bool idleStatus;
        float t = 0.0f; // We start from time 0.
        while(t < time && (!(idleStatus=_animator.GetBool("isIdle")))){
            t += Time.deltaTime;    // Framerate-indipendent cycle.
            this.changeRotation(Mathf.Lerp(0, rotation_y, t / time) % 360.0f); // Smooth rotation through lerping with deltaTime.
            yield return null;
        }
        _animator.SetBool("isIdle", true);  // Start animation IDLE.

        // Coroutine is finished; re-enable coroutine calls.
        isProgressiveChangeRotationCoroutineRunning=false;
    }

    /*
        This method should check for collisions.
     */
     void OnCollisionEnter(Collision collision) {
         // Name of the object who collided with: collision.transform.name.ToString()
         // Name of the layer of the object who collided with: collision.collider.gameObject.layer
         handleCollision(collision);
     }

    public void handleCollision(Collision collision, string who="default"){
        Debug.Log("Collider="+collision.contacts[0].thisCollider.gameObject.name);
        if(who.Equals("default")) who=transform.name;
        if(LayerMask.LayerToName(collision.collider.gameObject.layer).Equals("Box")){
            //Destroy(collision.collider.gameObject);
        }

        Debug.Log(collision.transform.name.ToString()+","+who);
    }

}