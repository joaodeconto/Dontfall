using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
	
	public float speed = 5.0f;
	public GameObject prefabRagdoll;
	
	private CharacterController controller;
	private Vector3 movement;
	
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		tag = "Player";
	}
	
	void Update () {
		Move ();
	}
	
	void Move () {
		movement.z = (Input.GetAxis("Horizontal") * Time.deltaTime * speed);
		controller.Move(movement);
	}
	
	public void Die () {
		GameObject ragdoll = Instantiate(prefabRagdoll, transform.position, transform.rotation) as GameObject;
		//ragdoll.tag = "Player";
		Destroy(gameObject);
	}
}
