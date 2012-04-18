using UnityEngine;
using System.Collections;

public class WindController : MonoBehaviour {
	
	private Rigidbody target;
	
	void Start () {
		enabled = false;
		audio.Play();
		audio.loop = true;
	}
	
	void Update () {
		Wind ();
	}
	
	public void Initialize (Rigidbody target) {
		this.target = target;
		enabled = true;
	}
	
	void Wind () {
		float pitch = 1 + target.velocity.magnitude / 100;
		audio.pitch = pitch;//pitch < 3f ? pitch : 3f;
	}
}
