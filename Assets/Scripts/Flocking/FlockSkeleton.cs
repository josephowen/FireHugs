using UnityEngine;
using System.Collections;

public class FlockSkeleton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Renderer[] renderers = transform.parent.parent.gameObject.GetComponentsInChildren<Renderer>();

		foreach (Renderer renderer in renderers)
		{
			renderer.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
