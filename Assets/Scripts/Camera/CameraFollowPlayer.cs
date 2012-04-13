using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour {
	
	private Transform target;
	private Vector3 distance;
	
	// Use this for initialization
	void Start () {
		distance = new Vector3(3f, 8f, -1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null) {
			transform.position = (target.position + distance);
		} else {
			target = GameObject.FindWithTag("Player").transform;
		}
	}
}
