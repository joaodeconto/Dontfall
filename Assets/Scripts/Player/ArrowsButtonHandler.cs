using UnityEngine;
using System.Collections;

public class ArrowsButtonHandler : MonoBehaviour {
	
	public enum Arrows {
		LEFT,
		RIGHT,
		FORWARD
	}
	
	public Arrows arrow;
	public Player player;
	
	private Camera camera;
	
	// Use this for initialization
	void Awake () {
		if (player == null) {
			Debug.LogError("Player Component not attached in the Arrows Button Handler script");
			Debug.Break();
		}
		
		camera = GameObject.FindWithTag("CameraArrows").camera;
		
		transform.GetChild(0).animation.Stop();
	}
	
	void OnMouseUpAsButton () {
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			if (hit.transform == transform) {
				switch (arrow) {
					case Arrows.LEFT:
						player.Move(1f);
						transform.GetChild(0).animation.Play();
						break;
					case Arrows.RIGHT:
						player.Move(-1f);
						transform.GetChild(0).animation.Play();
						break;
					case Arrows.FORWARD:
						player.Jump();
						camera.gameObject.SetActiveRecursively(false);
						transform.parent.gameObject.SetActiveRecursively(false);
						break;
					default:
						Debug.LogWarning("Arrow Type not selected");
						break;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetMouseButton(0)) {
//			Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//			if (Physics.Raycast(ray, out hit)) {
//				if (hit.transform == transform) {
//					switch (arrow) {
//						case Arrows.FORWARD:
//							transform.localScale = new Vector3(1, 1, 1) + (transform.localScale * (player.PrepareToJump() / 50));
//							break;
//					}
//				}
//			}
//		}
		/*if (Input.GetMouseButtonUp(0)) {
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				if (hit.transform == transform) {
					switch (arrow) {
						case Arrows.LEFT:
							player.Move(1f);
							transform.GetChild(0).animation.Play();
							break;
						case Arrows.RIGHT:
							player.Move(-1f);
							transform.GetChild(0).animation.Play();
							break;
						case Arrows.FORWARD:
							player.Jump();
							camera.gameObject.SetActiveRecursively(false);
							transform.parent.gameObject.SetActiveRecursively(false);
							break;
						default:
							Debug.LogWarning("Arrow Type not selected");
							break;
					}
				}
			}
		}*/
	}
}
