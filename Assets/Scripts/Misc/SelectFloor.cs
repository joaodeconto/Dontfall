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
	private int enableLevels;
	
	#region Rects and GUIStyles
	private Rect windowLevel;
	private float widthButton, heightButton, spaceButton;
	private GUIStyle buttonStyle;
	#endregion
	
	// Use this for initialization
	void Start () {
		
		ScreenUtils.Initialize(960, 600);
		
		buttonStyle = new GUIStyle();
		buttonStyle.normal.textColor = Color.white;
		buttonStyle.alignment = TextAnchor.MiddleCenter;
		buttonStyle.fontStyle = FontStyle.Bold;
		
		AdjustScreen ();
		
		enableLevels = PlayerPrefs.GetInt("Enable Levels", 0);
		
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
		
		//System.Array.Reverse(floors);
	}
	
	
	// Update is called once per frame
	void OnGUI () {
		if (ScreenUtils.ScreenSizeChange()) {
			AdjustScreen ();
		}
		
		GUILayout.BeginArea(windowLevel);
		int j = 0;
		for (int i = 0; i < floors.Length; i++) {
			if (i > enableLevels) GUI.enabled = false;
			if (j == 0) GUILayout.BeginHorizontal();
			if (GUILayout.Button(floors[i].number, buttonStyle, GUILayout.Width(widthButton), GUILayout.Height(heightButton))) {
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
				PlayerPrefs.SetInt("Actual Level", i);
				enabled = false;
			}
			j++;
			if (j == 5 || i == floors.Length-1) {
				GUILayout.EndHorizontal();
				j = 0;
			}
			GUILayout.Space(spaceButton);
		}
		GUILayout.EndArea();
	}
	
	void AdjustScreen () {
		windowLevel = ScreenUtils.ScaledRectInSenseHeight(50, 10, 960, 600);
		widthButton = ScreenUtils.ScaleHeight(120);
		heightButton = ScreenUtils.ScaleHeight(100);
		spaceButton = ScreenUtils.ScaleHeight(50);
		buttonStyle.fontSize = ScreenUtils.ScaleHeightInt(32);
	}
}
