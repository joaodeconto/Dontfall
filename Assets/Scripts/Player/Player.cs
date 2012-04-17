using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
	
	public float speed = 5.0f;
	public GameObject prefabRagdoll;
	public GameObject blood;
	public float maximumForce = 15f;
	
	private CharacterController controller;
	private Vector3 movement;
	private float force;
	private bool clicked;
	
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		tag = "Player";
		clicked = false;
	}
	
	void Update () {
		Move ();
	}
	
	void OnGUI () {
		if (force != 0) {
			GUILayout.Label("FORCE: " + (int)((force / maximumForce) * 100));
		}
	}
	
	/*void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.transform.tag != "Player") {
			CallRagdoll();
		}
	}*/
	
	void Move () {
		movement.z = (Input.GetAxis("Horizontal") * Time.deltaTime * speed);
		movement.x = (Input.GetAxis("Vertical") * Time.deltaTime * speed);
		if (!clicked) {
			if (!Input.GetKey(KeyCode.Space)) {
				controller.Move(movement);
			}
			else {
				if (force < maximumForce)
					force += 0.1f;
			}
			if (Input.GetKeyUp(KeyCode.Space)) {
				//movement.y = (force / 100);
				clicked = true;
			}
		} else {
			CallRagdoll();
			//movement.y -= 20f * Time.deltaTime;;
			//controller.Move(movement);
		}
	}
	
	public void CallRagdoll () {
		GameObject ragdoll = Instantiate(prefabRagdoll, transform.position, transform.rotation) as GameObject;
		CollisionPartOfBody[] collisions = ragdoll.GetComponentsInChildren<CollisionPartOfBody>();
		foreach (CollisionPartOfBody c in collisions) {
			c.Initialize(blood);
			print("");
			//if ( c.GetComponent<CharacterJoint>() != null) c.GetComponent<CharacterJoint>().breakForce = 100 * c.rigidbody.mass;
			if ( c.rigidbody != null ) c.rigidbody.velocity = (new Vector3(movement.x * force, Mathf.Abs(movement.x) * force, movement.z * force) * c.rigidbody.mass);
		}
		//GameObject.FindWithTag("MainCamera").GetComponent<CameraFollowPlayer>().target = collisions[0].transform;
		//ragdoll.tag = "Player";
		Destroy(gameObject);
	}
	
}
