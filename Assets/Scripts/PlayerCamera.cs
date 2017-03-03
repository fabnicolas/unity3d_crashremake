using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target_transform;
    private float vertical;
    private float rotation_speed = 4.0f;

    private Vector3 camera_offset;
    void Start()
    {
        vertical = transform.eulerAngles.x;
        camera_offset = transform.position;
    }

    void Update()
    {
        transform.position = target_transform.position + camera_offset;

        float mouseVertical = Input.GetAxis("Mouse Y");
        vertical = (vertical - rotation_speed * mouseVertical) % 360f;
        vertical = Mathf.Clamp(vertical, -5, 20);
        transform.localRotation = Quaternion.AngleAxis(vertical, Vector3.right);
    }
}