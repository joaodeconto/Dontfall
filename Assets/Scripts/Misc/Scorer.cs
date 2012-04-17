using UnityEngine;
using System.Collections;

public class Scorer : MonoBehaviour {
	
	private float higherVelocity;
	private CollisionPartOfBody[] collisions;
	
	void Start () {
		enabled = false;
	}
	
	void OnGUI () {
		GUILayout.BeginArea(new Rect(10, 10, 200, Screen.height));
			//string higher = System.String.Format("{0:0,0}", higherVelocity);
			GUILayout.Label("Higher Velocity: " + higherVelocity.ToString("n2"));
			GUILayout.Label("Number of Fractures:");
			foreach (CollisionPartOfBody c in collisions) {
				GUILayout.Label(c.gameObject.name + ": " + c.fracture);
			}
		GUILayout.EndArea();
	}
	
	public void CalculatePoints (float higherVelocity, CollisionPartOfBody[] collisions) {
		this.higherVelocity = higherVelocity;
		this.collisions = collisions;
		enabled = true;
	}
	
}
