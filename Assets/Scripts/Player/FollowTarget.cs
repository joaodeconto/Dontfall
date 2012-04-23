using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public Transform target;
	
	public Vector3 distance {get; set;}
	// Use this for initialization
	void Start () {
		//if (target == null) target = GameObject.FindWithTag("Player").transform;
		
		//distance = target.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.position = (target.position + distance);
		} else {
			if (GameObject.FindWithTag("Player") != null) {
				target = GameObject.FindWithTag("Player").transform;
				distance = (transform.position - target.position);
			}
		}
	}
}
