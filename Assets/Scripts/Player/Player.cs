using UnityEngine;
using System.Collections;

[AddComponentMenu("Dont Fall/Player/Player")]
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
	private WindController windController;
	private bool jump = false;
	private float lastHeight;
	
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		windController = GameObject.Find("WindAudio").GetComponent<WindController>();
		tag = "Player";
		clicked = false;
		Application.runInBackground = true;
	}
	
	void Update () {
		Move ();
	}
	
	void OnGUI () {
		if (force != 0) {
			GUILayout.Label("FORCE: " + (int)((force / maximumForce) * 100));
		}
	}
	
	void OnCollisionEnter(Collision hit) {
		if (hit.transform.tag != "Player") {
			if (jump) {
			//	CallRagdoll();
			}
		}
	}
	
	void Move () {
		if (!clicked) {
			rigidbody.velocity = (new Vector3(0, 0, 0)); 
			movement.z = (Input.GetAxis("Horizontal") * Time.deltaTime * speed);
			movement.x = (Input.GetAxis("Vertical") * Time.deltaTime * speed);
			if (!Input.GetKey(KeyCode.Space)) {
				//controller.Move(new Vector3(0, movement.y, movement.z));
				controller.Move(movement);
				if (Input.GetAxisRaw("Horizontal") != 0) {
					animation.CrossFade("Strafe");
				} else {
					animation.CrossFade("Idle");
				}
			}
			else {
				animation.CrossFade("Idle");
				if (force < maximumForce)
					force += 0.1f;
				controller.Move(Vector3.zero);
			}
			if (Input.GetKeyUp(KeyCode.Space)) {
				//movement.y = (force / 100);
				clicked = true;
				animation.CrossFade("Jump");
				//lastHeight = transform.position.y;
			}
		} else {
			if (!jump) {
				if (rigidbody != null && (animation["Jump"].time / animation["Jump"].length) > 0.24f) {
					float z = movement.x > 0 ? movement.z * force : 0;
					float x = movement.x > 0 ? movement.x * force : 0.1f;
					rigidbody.velocity = (new Vector3(x, 0.2f * force, z) * 3); 
					jump = true;
					controller.enabled = false;
					GetComponent<CapsuleCollider>().enabled = true;
				}
			}
			if (transform.position.y < lastHeight && !animation.IsPlaying("Jump")) CallRagdoll();
			else lastHeight = transform.position.y;
			//movement.y -= 20f * Time.deltaTime;;
			//controller.Move(movement);
		}
	}
	
	
	public void CallRagdoll () {
		GameObject ragdoll = Instantiate(prefabRagdoll, transform.position, transform.rotation) as GameObject;
		Transform[] ragdollChilds = ragdoll.GetComponentsInChildren<Transform>();
		Transform[] childs = GetComponentsInChildren<Transform>();
		for (int i = 0; i != ragdollChilds.Length; ++i) {
			ragdollChilds[i].position = childs[i].position;
			ragdollChilds[i].rotation = childs[i].rotation;
		}
		CollisionPartOfBody[] collisions = ragdoll.GetComponentsInChildren<CollisionPartOfBody>();
		foreach (CollisionPartOfBody c in collisions) {
			c.Initialize(blood);
			//if ( c.GetComponent<CharacterJoint>() != null) c.GetComponent<CharacterJoint>().breakForce = 100 * c.rigidbody.mass;
			if ( c.isMain ) windController.Initialize(c.rigidbody);
			if ( c.rigidbody != null ) c.rigidbody.velocity = rigidbody.velocity;
			if ( c.rigidbody != null ) c.rigidbody.AddTorque(new Vector3(0, 0, (movement.x * force) * -10000) * c.rigidbody.mass);
			
		}
		//GameObject.FindWithTag("MainCamera").GetComponent<CameraFollowPlayer>().target = collisions[0].transform;
		//ragdoll.tag = "Player";
		Destroy(gameObject);
	}
	
}
