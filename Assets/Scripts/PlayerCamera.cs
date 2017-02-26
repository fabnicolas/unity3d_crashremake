using UnityEngine;

public class PlayerCamera : MonoBehaviour{
    public Transform player_transform;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
         float x = player_transform.position.x;
         float y = player_transform.position.y + 2; // +2: camera a bit heigher than the character. Or -2, depends on from which side you want to look.
         float z = player_transform.position.z + 5; // +5: distance between character and camera
         transform.position = new Vector3(x,y,z);
         transform.rotation = new Quaternion(0,180,0,0); // rotate around y-Axis to look down
    }
}