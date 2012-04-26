using UnityEngine;
using System.Collections;

public class SelectFloor : MonoBehaviour {
	
	public class Floor {
		public string number;
		public Vector3 position;
		
		public Floor (string number, Vector3 position) {
			this.number = number;
			this.position = position;
		}
	}
	
	public Transform selectFloor;
	public GameObject player;
	public GameObject directions;
	
	private Floor[] floors;
	private CameraFollowPlayer cameraFollowPlayer;
	
	// Use this for initialization
	void Start () {
//		if (selectFloor != null) {
//			Debug.LogError("Not attached SelectFloor in script");
//			return;
//		}
		
		player.SetActiveRecursively(false);
		directions.SetActiveRecursively(false);
		
		cameraFollowPlayer = GetComponent<CameraFollowPlayer>();
		cameraFollowPlayer.enabled = false;
		
		floors = new Floor[selectFloor.childCount];
		for (int j = 0; j < floors.Length; j++) {
			int i = 0;
			foreach (Transform childFloors in selectFloor.transform) {
				if (System.Convert.ToInt32(selectFloor.GetChild(j).name) > System.Convert.ToInt32(childFloors.name)) {
					i++;
				}
			}
			floors[j] = new Floor(selectFloor.GetChild(i).name, selectFloor.GetChild(i).localPosition);
		}
		
		System.Array.Reverse(floors);
		
	}
	
	// Update is called once per frame
	void OnGUI () {
//		if (selectFloor != null) {
//			Debug.LogError("Not attached SelectFloor in script");
//			return;
//		}
		
		GUILayout.BeginArea(new Rect(10, 10, 200, 1000));
		for (int i = 0; i < floors.Length; i++) {
			if (GUILayout.Button(floors[i].number, GUILayout.Width(40), GUILayout.Height(30))) {
				Vector3 distance = (cameraFollowPlayer.transform.position - player.transform.position);
				player.transform.position = floors[i].position;
				player.SetActiveRecursively(true);
				cameraFollowPlayer.enabled = true;
				cameraFollowPlayer.target = player.transform;
				cameraFollowPlayer.distance = (distance - player.transform.position) +
												player.transform.position;
				directions.SetActiveRecursively(true);
				directions.GetComponent<FollowTarget>().distance = (distance - player.transform.position) +
												player.transform.position;
				enabled = false;
			}
		}
		GUILayout.EndArea();
	}
}
