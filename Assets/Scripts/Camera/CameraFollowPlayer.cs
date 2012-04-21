using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour {
	
	public Transform target {set; get;}
	public Vector3 distance {set; get;}
	
	// Use this for initialization
	void Start () {
		//distance = new Vector3(3f, 8f, -1f);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown (KeyCode.Backspace)) {  
		    Application.LoadLevel (0);  
		}  
		
		if (target != null) {
			transform.position = (target.position + distance);
		} else {
			target = GameObject.FindWithTag("Player").transform;
			distance = (transform.position - target.position);
		}
	}
}
