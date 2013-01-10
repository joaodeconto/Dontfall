using UnityEngine;
using System.Collections;

public class Hydrant : MonoBehaviour {
	
	public GameObject waterHydrant;
	
	private bool brokenHydrant;
	
	// Use this for initialization
	void Start ()
	{
		brokenHydrant = false;
	}
	
	void OnCollisionEnter (Collision collision)
	{
		if (!brokenHydrant)
		{
			if (collision.transform.CompareTag ("Player") || collision.transform.CompareTag ("Ragdoll"))
			{
				if (collision.impactForceSum.magnitude > 5f)
				{
					brokenHydrant = true;
					GameObject.Find("Scorer").GetComponent<Scorer>().ExtraPoints += 200;
					Instantiate (waterHydrant, transform.position, waterHydrant.transform.rotation);
				}
			}
		}
	}
}
