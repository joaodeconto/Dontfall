using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
	
	public float speed = 5.0f;
	public GameObject prefabRagdoll;
	public GameObject blood;
	
	private CharacterController controller;
	private Vector3 movement;
	private float force;
	
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		tag = "Player";
	}
	
	void Update () {
		Move ();
	}
	
	void Move () {
		if (!Input.GetKey(KeyCode.Space)) {
			movement.z = (Input.GetAxis("Horizontal") * Time.deltaTime * speed);
			movement.x = (Input.GetAxis("Vertical") * Time.deltaTime * speed);
			controller.Move(movement);
		}
		else {
			print(force);
			force += 1f;
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			movement.z = (Input.GetAxis("Horizontal") * Time.deltaTime * speed);
			movement.x = (Input.GetAxis("Vertical") * Time.deltaTime * speed);
			CallRagdoll();
		}
	}
	
	public void CallRagdoll () {
		GameObject ragdoll = Instantiate(prefabRagdoll, transform.position, transform.rotation) as GameObject;
		CollisionPartOfBody[] collisions = ragdoll.GetComponentsInChildren<CollisionPartOfBody>();
		foreach (CollisionPartOfBody c in collisions) {
			c.Initialize(blood);
			//if ( c.GetComponent<CharacterJoint>() != null) c.GetComponent<CharacterJoint>().breakForce = 100 * c.rigidbody.mass;
			if ( c.rigidbody != null ) c.rigidbody.velocity = (new Vector3(movement.x * force, Mathf.Abs(movement.x) * force, movement.z * force) * c.rigidbody.mass);
		}
		//GameObject.FindWithTag("MainCamera").GetComponent<CameraFollowPlayer>().target = collisions[0].transform;
		//ragdoll.tag = "Player";
		Destroy(gameObject);
	}
}
