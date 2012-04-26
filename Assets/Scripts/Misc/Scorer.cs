using UnityEngine;
using System.Collections;

public class Scorer : MonoBehaviour {
	
	private float higherVelocity;
	private CollisionPartOfBody[] collisions;
	private float totalFractures;
	private GUIStyle titleStyle;
	
	void Start () {
		enabled = false;
		
		titleStyle = new GUIStyle();
		titleStyle.normal.textColor = Color.white;
		titleStyle.alignment = TextAnchor.MiddleCenter;
	}
	
	void OnGUI () {
		GUILayout.BeginArea(new Rect(10, 10, 200, 400), new GUIStyle("box"));
			//string higher = System.String.Format("{0:0,0}", higherVelocity);
			GUILayout.Label("SCORE:", titleStyle);
			GUILayout.Label("Higher Velocity: " + higherVelocity.ToString("n2") + " Km/s");
			GUILayout.Label("Number of Fractures: " + totalFractures);
			foreach (CollisionPartOfBody c in collisions) {
				GUILayout.Label(c.gameObject.name + ": " + c.fracture);
			}
		GUILayout.EndArea();
	}
	
	public void CalculatePoints (float higherVelocity, CollisionPartOfBody[] collisions) {
		this.higherVelocity = higherVelocity;
		this.collisions = collisions;
		totalFractures = 0;
		foreach (CollisionPartOfBody collision in collisions) {
			if (collision.GetComponent<CharacterJoint>())
				Destroy(collision.GetComponent<CharacterJoint>());
			Destroy(collision.rigidbody);
			totalFractures += collision.fracture;
		}
		enabled = true;
	}
	
}
