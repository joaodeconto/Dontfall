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
	
	// Use this for initialization
	void Start () {
		if (player == null) {
			Debug.LogError("Player Component not attached in the Arrows Button Handler script");
			Debug.Break();
		}
		
		transform.GetChild(0).animation.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				print(hit.transform.name);
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
							player.Move(0);
							transform.GetChild(0).animation.Play();
							break;
						default:
							Debug.LogWarning("Arrow Type not selected");
							break;
					}
				}
			}
		}
	}
}
