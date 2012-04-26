using UnityEngine;
using System.Collections;

[AddComponentMenu("Dont Fall/Cars/Car")]
public class Cars : MonoBehaviour {
	
	public Transform[] waypoints;
	private float speed = 10f;
	
	public bool isMoving {get; private set;}
	public bool objectOnTheWay {get; private set;}
	private int indexCurrentWaypoint;
	private Vector3 moving;
	private float maxSpeed;
	
	private MeshCollider colliderCar;
	private Vector3 sideRight, sideLeft, center;
	private Ray[] rays;
	private RaycastHit hit;
	private int side;
	private GameObject[] objects;
	
	void Start () {
		indexCurrentWaypoint = 0;
		colliderCar = transform.GetComponentInChildren<MeshCollider>();
		//rays = new Ray[3];
		objectOnTheWay = false;
		isMoving = true;
		maxSpeed = speed;
		AddCampoVisao();
		ChangeSide();
	}
	
	void OnTriggerStay (Collider other) {
		if (!other.tag.Equals("Scenario")) {
			if (other.tag.Equals("Car")) {
				audio.pitch = 1f;
				objectOnTheWay = false;
				isMoving = true;
				if (other.transform.parent.GetComponent<Cars>().objectOnTheWay) {
					audio.pitch = 0.1f;
					objectOnTheWay = true;
				}
				if (other.transform.parent.GetComponent<Cars>().isMoving) {
					audio.pitch = 0.1f;
					isMoving = false;
				}
			} else {
				audio.pitch = 0.1f;
				objectOnTheWay = true;
			}
		}
    }
	
	void OnTriggerExit (Collider other) {
		objectOnTheWay = false;
		isMoving = true;
	}
	
	void Update () {
		
		if (!objectOnTheWay && isMoving) {
			Move();
		}
		
		/*rays[0] = new Ray(transform.position + (sideRight), transform.forward);
		rays[1] = new Ray(transform.position + (sideLeft), transform.forward);
		rays[2] = new Ray(transform.position + (center), transform.forward);
		for (int i = 0; i != rays.Length; ++i) {
			Debug.DrawRay(rays[i].origin, rays[i].direction * 2.5f, Color.red);
			if(Physics.Raycast(rays[i], out hit, 2.5f)) {
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
				}
				else if (hit.transform.tag.Equals("Scenario")) {
					continue;
				} else {
					audio.pitch = 0.1f;
					objectOnTheWay = true;
					return;
				}
			}
		}
		audio.pitch = 1;
		objectOnTheWay = false;
		isMoving = true;
		Move();*/
	}
	
	void AddCampoVisao () {
		gameObject.AddComponent<BoxCollider>();
		BoxCollider bc = gameObject.GetComponent<BoxCollider>();
		bc.size = new Vector3(colliderCar.bounds.size.z, colliderCar.bounds.size.y, colliderCar.bounds.size.x*0.5f);
		bc.center = new Vector3(0, (bc.size.y/2)+0.3f, bc.size.z);
		bc.isTrigger = true;
		gameObject.AddComponent<Rigidbody>();
		gameObject.rigidbody.isKinematic = true;
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
		
		/*if (Mathf.Approximately(transform.eulerAngles.y, 90)) {
			side = 0;
		}
		if (Mathf.Approximately(transform.eulerAngles.y, 270)) {
			side = 1;
		}
		if (Mathf.Approximately(transform.eulerAngles.y, 0)) {
			side = 2;
		}
		if (Mathf.Approximately(transform.eulerAngles.y, 180)) {
			side = 3;
		}
		
		if (side == 0) {
			sideRight = new Vector3( colliderCar.bounds.size.x - colliderCar.bounds.extents.x*1.25f,
									-colliderCar.bounds.size.y + 1.5f,
	                            	 colliderCar.bounds.size.z - colliderCar.bounds.extents.z*1.25f);
			sideLeft = new Vector3(  colliderCar.bounds.size.x - colliderCar.bounds.extents.x*1.25f,
									-colliderCar.bounds.size.y + 1.5f,
			                    	-colliderCar.bounds.size.z + colliderCar.bounds.extents.z*1.25f);
			
			center = new Vector3( colliderCar.bounds.size.x - colliderCar.bounds.extents.x*1.25f,
								 -colliderCar.bounds.size.y + 1.5f,
					              0);
		} else if (side == 1) {
			sideRight = new Vector3(-colliderCar.bounds.size.x + colliderCar.bounds.extents.x*1.25f,
									-colliderCar.bounds.size.y + 1.5f,
	                            	 colliderCar.bounds.size.z - colliderCar.bounds.extents.z*1.25f);
			sideLeft = new Vector3( -colliderCar.bounds.size.x + colliderCar.bounds.extents.x*1.25f,
									-colliderCar.bounds.size.y + 1.5f,
			                    	-colliderCar.bounds.size.z + colliderCar.bounds.extents.z*1.25f);
			
			center = new Vector3(-colliderCar.bounds.size.x + colliderCar.bounds.extents.x*1.25f,
								 -colliderCar.bounds.size.y + 1.5f,
					              0);
		} else if (side == 2) {
			sideRight = new Vector3( colliderCar.bounds.size.x - colliderCar.bounds.extents.x*1.25f,
									-colliderCar.bounds.size.y + 1.5f,
	                            	 colliderCar.bounds.size.z - colliderCar.bounds.extents.z*1.25f);
			sideLeft = new Vector3( -colliderCar.bounds.size.x + colliderCar.bounds.extents.x*1.25f,
									-colliderCar.bounds.size.y + 1.5f,
			                    	 colliderCar.bounds.size.z - colliderCar.bounds.extents.z*1.25f);
			
			center = new Vector3( 0,
								 -colliderCar.bounds.size.y + 1.5f,
					              colliderCar.bounds.size.z - colliderCar.bounds.extents.z*1.25f);
		} else {
			sideRight = new Vector3( colliderCar.bounds.size.x - colliderCar.bounds.extents.x*1.25f,
									-colliderCar.bounds.size.y + 1.5f,
	                            	-colliderCar.bounds.size.z + colliderCar.bounds.extents.z*1.25f);
			sideLeft = new Vector3( -colliderCar.bounds.size.x + colliderCar.bounds.extents.x*1.25f,
									-colliderCar.bounds.size.y + 1.5f,
			                    	-colliderCar.bounds.size.z + colliderCar.bounds.extents.z*1.25f);
			
			center = new Vector3( 0,
								 -colliderCar.bounds.size.y + 1.5f,
					             -colliderCar.bounds.size.z - colliderCar.bounds.extents.z*1.25f);
		}*/
	}
}
