using UnityEngine;
using System.Collections;

[AddComponentMenu("Dont Fall/Player/Blood")]
public class CollisionPartOfBody : MonoBehaviour {
	
	public bool isBlood = true;
	public bool isMain = false;
	
	public float higherVelocity {private set; get;}
	public GameObject blood {set; get;}
	public int fracture {private set; get;}
	
	private float timer;
	
	void Update () {
		if (isMain) {
			if (rigidbody.velocity.magnitude > higherVelocity) higherVelocity = rigidbody.velocity.magnitude;
			if (rigidbody.velocity.magnitude < 1) {
				timer += Time.deltaTime;
				if (timer > 2f) {
					EndGame ();
				}
			} else timer = 0;
			if (rigidbody.velocity.magnitude == 0) EndGame ();
		}
	}
	
	void OnCollisionEnter (Collision collision) {
		if (!rigidbody)
			return;
		
		if (collision.transform.tag != "Player") {
			
			float impactForce = collision.relativeVelocity.magnitude * rigidbody.mass;
			
			print(transform.name + " : " + impactForce);
			
			if (impactForce > 30 * rigidbody.mass || 
				(transform.name.Equals("Head_R") && impactForce > 10 * rigidbody.mass))
				Instantiate (blood, transform.position, transform.rotation);
			
			if (impactForce > 20 * rigidbody.mass) {
				fracture++;
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag != "Player") {
			fracture++;
		}
    }
	
	/*void OnJointBreak(float breakForce) {
		Transform[] bones = GameObject.FindWithTag("Ragdoll").GetComponent<SkinnedMeshRenderer>().bones;
		for (int i = 0; i != bones.Length; ++i) {
			if (bones[i] == transform) bones[i] = null;
		}
    }*/
	
	public void Initialize (GameObject blood) {
		this.blood = blood;
		fracture = 0;
		higherVelocity = 0;
	}
	
	void EndGame () {
		CollisionPartOfBody[] collisions = transform.parent.GetComponentsInChildren<CollisionPartOfBody>();
		GameObject.Find("Scorer").GetComponent<Scorer>().CalculatePoints(higherVelocity, collisions);
	}
}
