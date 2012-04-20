using UnityEngine;
using System.Collections;

[AddComponentMenu("Dont Fall/Player/Collision Part Of Body")]
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
			} else {
				timer = 0;
			}
			if (rigidbody.velocity.magnitude == 0) EndGame ();
			
			if (rigidbody.velocity.magnitude > 5) {
				GetComponentInChildren<AudioSource>().volume = Mathf.Clamp( rigidbody.velocity.magnitude / 100,
																			0, 1);
			} else {
				GetComponentInChildren<AudioSource>().volume = 0;
			}
		}
	}
	
	void OnCollisionEnter (Collision collision) {
		if (!rigidbody)
			return;
		
		if (!collision.transform.tag.Equals("Player")) {
			
			float impactForce = collision.relativeVelocity.magnitude * rigidbody.mass;
			
			print(transform.name + " : " + impactForce);
			
			if (impactForce > ForceToApply(25) || 
				(transform.name.Equals("Neck_R") && impactForce > ForceToApply(10)))
				Instantiate (blood, transform.position, transform.rotation);
			
			if (impactForce > ForceToApply(10)) {
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
	
	float ForceToApply (float maxForce) {
		return (maxForce * rigidbody.mass); /*/ (fracture + 1);*/ 
	}
	
	void NumberOfFractures () {
	}
	
	void EndGame () {
		CollisionPartOfBody[] collisions = transform.parent.GetComponentsInChildren<CollisionPartOfBody>();
		GameObject.Find("Scorer").GetComponent<Scorer>().CalculatePoints(higherVelocity, collisions);
		GameObject.Find("WindAudio").audio.volume = 0.1f;
		enabled = false;
	}
}
