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
	private bool unconscious;
	private AudioSource voice;
	
	void Start () {
		if (isMain) {
			voice = GetComponentInChildren<AudioSource>();
		}
	}
	
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
			
			if (rigidbody.velocity.magnitude > 5 && !unconscious) {
				voice.volume = Mathf.Clamp( rigidbody.velocity.magnitude / 100,
																			0, 1);
			} else {
				voice.volume = 0;
			}
		}
	}
	
	void OnCollisionEnter (Collision collision) {
		if (!rigidbody)
			return;
		
		float impactForce = collision.relativeVelocity.magnitude * rigidbody.mass;
		
		if (collision.transform.tag.Equals("Car")) {
			print(-impactForce/11);
			collision.transform.GetComponent<CarDamage>().OnMeshForce(transform.position, -impactForce/11);
		}
			
		if (!collision.transform.tag.Equals("Player")) {
			if (impactForce > ForceToApply(30))
				Instantiate (blood, transform.position, transform.rotation);
			
			if (transform.name.Equals("Neck_R") && impactForce > ForceToApply(15)) {
				Instantiate (blood, transform.position, transform.rotation);
				if (!unconscious) {
					unconscious = true;
					SendMessageUpwards("Unconscious");
				}
			}
			
			if (impactForce > ForceToApply(10)) {
				GameObject brokenBone = new GameObject("Broken Bone");
				brokenBone.AddComponent<AudioSource>();
				brokenBone.audio.dopplerLevel = 0;
				brokenBone.audio.playOnAwake = false;
				brokenBone.audio.clip = Resources.LoadAssetAtPath("Assets/Sounds/Bone Broken.mp3", typeof(AudioClip)) as AudioClip;
				brokenBone.audio.Play();
//				brokenBone.AddComponent<AutoDestroy>();
//				brokenBone.GetComponent<AutoDestroy>().timer = brokenBone.audio.time + 0.1f;
				brokenBone.transform.position = transform.position;
				brokenBone.transform.parent = transform;
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
	
	void Unconscious () {
		unconscious = true;
	}
	
	void EndGame () {
		CollisionPartOfBody[] collisions = transform.parent.GetComponentsInChildren<CollisionPartOfBody>();
		GameObject.Find("Scorer").GetComponent<Scorer>().CalculatePoints(higherVelocity, collisions);
		GameObject.Find("WindAudio").audio.volume = 0.1f;
		enabled = false;
	}
}
