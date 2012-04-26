using UnityEngine;
using System.Collections;

public class WindController : MonoBehaviour {
	
	private Rigidbody target;
	private Vector3 oldPosition;
	
	void Start () {
		enabled = false;
		audio.Play();
		audio.loop = true;
		oldPosition = transform.position;
	}
	
	void Update () {
		Wind ();
	}
	
	public void Initialize (Rigidbody target) {
		this.target = target;
		enabled = true;
	}
	
	void Wind () {
		if (target != null) {
			Vector3 newPosition = target.position;
			newPosition.y = oldPosition.y;
			transform.position = newPosition;
			float pitch = 1 + target.velocity.magnitude / 100;
			audio.pitch = pitch;//pitch < 3f ? pitch : 3f;
		}
	}
}
