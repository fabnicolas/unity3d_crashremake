using UnityEngine;

public class PlayerController : MonoBehaviour{
	public enum PlayerMovementStatus : byte{
        IDLE = 0,
		WALKING = 1,
		RUNNING = 2,
		WALKING_BACK = 3,
		WALKING_RIGHT = 4,
		WALKING_LEFT = 5
	}

    private PlayerMovementStatus movement_status;
    private Animator _animator;

    private Vector3 movement_vector;

    private float horizontal;
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        /*
        // Set player position to the center of the screen.

        position_player.x = 0.5f;
		position_player.y = 0.5f;
		position_player.z = 0f;
        */
        _animator=this.GetComponent<Animator>(); // Gets the animator to the attached GO.
        horizontal = transform.eulerAngles.y;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
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

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        float mouseHorizontal = Input.GetAxis("Mouse X");
		horizontal = (horizontal + 0.4f * mouseHorizontal) % 360f;
		transform.rotation = Quaternion.AngleAxis(horizontal, Vector3.up); // Handles mouse X orientation
        
        Debug.Log("mv="+movement_vector);
        if(movement_vector != Vector3.zero)
            transform.Translate((movement_vector=movement_vector*Time.deltaTime));
        else
            transform.Translate(0,0,0);
    }


}