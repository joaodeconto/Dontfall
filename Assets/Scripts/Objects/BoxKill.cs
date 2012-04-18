using UnityEngine;
using System.Collections;

public class BoxKill : MonoBehaviour {
	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag.Equals("Player")) {
			if (collider.gameObject.GetComponent<Player>() != null) {
				collider.gameObject.GetComponent<Player>().CallRagdoll();
			}
		}
	}
}