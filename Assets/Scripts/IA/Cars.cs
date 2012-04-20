using UnityEngine;
using System.Collections;

[AddComponentMenu("Dont Fall/Cars/Car")]
public class Cars : MonoBehaviour {
	
	public Transform[] waypoints;
	private float speed = 20f;
	
	public bool isMoving {get; private set;}
	public bool objectOnTheWay {get; private set;}
	private int indexCurrentWaypoint;
	private Vector3 moving;
	private float maxSpeed;
	
	private Mesh colliderCar;
	private Vector3 sideRight, sideLeft;
	private Ray[] rays;
	private RaycastHit hit;
	
	void Start () {
		indexCurrentWaypoint = 0;
		colliderCar = transform.GetComponentInChildren<MeshFilter>().mesh;
		rays = new Ray[2];
		objectOnTheWay = false;
		isMoving = true;
		maxSpeed = speed;
		ChangeSide();
	}
	
	void Update () {
		rays[0] = new Ray(transform.position + (sideRight), transform.forward);
		rays[1] = new Ray(transform.position + (sideLeft), transform.forward);
		for (int i = 0; i != rays.Length; ++i) {
//			Debug.DrawRay(rays[i].origin, rays[i].direction * 7.5f);
			if(Physics.Raycast(rays[i], out hit, 7.5f)) {
				if (hit.transform.tag.Equals("Car")) {
					if (hit.transform != transform.GetChild(0)) {
						if (hit.transform.parent.GetComponent<Cars>().objectOnTheWay) {
							audio.pitch = 0.1f;
							objectOnTheWay = true;
							return;
						}
						if (hit.transform.parent.GetComponent<Cars>().isMoving) {
							audio.pitch = 0.1f;
							isMoving = false;
							return;
						}
					}
				} else {
					audio.pitch = 0.1f;
					objectOnTheWay = true;
					return;
				}
//				if (hit.transform.parent != null) {
//					if (hit.transform.parent != transform) {
//						if (hit.transform.tag.Equals("Car")) {
//							if (i == 0)	return;
//						} else return;
//					}
//				} else return;
			}
		}
		audio.pitch = 1;
		objectOnTheWay = false;
		isMoving = true;
		Move();
	}
	
	void Move () {
		moving = (waypoints[indexCurrentWaypoint].position - transform.position);
		moving.y = 0;
		if (moving.magnitude > 1f) {
			transform.position += (moving.normalized * Time.deltaTime * speed);
		} else {
			indexCurrentWaypoint = (indexCurrentWaypoint + 1) % waypoints.Length;
			ChangeSide();
		}
	}
	
	void ChangeSide () {
		Vector3 distance = (waypoints[indexCurrentWaypoint].position - transform.position);
		distance.y = 0;
		if (Mathf.Abs(distance.z) > Mathf.Abs(distance.x)) {
			transform.eulerAngles = (Vector3.up * (Mathf.Abs(1 - Mathf.Sign(distance.z)) / 2) * 180);
		} else {
			transform.eulerAngles = Vector3.up * ((2 - Mathf.Sign(distance.x)) * 90);
		}
		sideRight = new Vector3( colliderCar.bounds.size.x * Mathf.Sign(distance.z),
								-colliderCar.bounds.size.y + 0.25f,
                            	colliderCar.bounds.size.z * -Mathf.Sign(distance.x));
		sideLeft = new Vector3( colliderCar.bounds.size.x * -Mathf.Sign(distance.z),
								-colliderCar.bounds.size.y + 0.25f,
                            	colliderCar.bounds.size.z * Mathf.Sign(distance.x));
	}
}
