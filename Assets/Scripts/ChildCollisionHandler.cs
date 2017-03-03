using UnityEngine;

public class ChildCollisionHandler : MonoBehaviour{
	void Start ()
	{

	}

	void Update ()
	{

	}

    void OnCollisionEnter(Collision other)
    {
        transform.parent.GetComponent<PlayerController>().handleCollision(other, "child..."+transform.name);
    }
}