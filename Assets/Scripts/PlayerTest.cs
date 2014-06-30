using UnityEngine;
using System.Collections;

public class PlayerTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.W))
		{
			scaleBy(1.01f);
		}
		else if (Input.GetKey(KeyCode.S))
		{
			scaleBy(0.99f);
		}
	}

	void scaleBy(float scaleFactor)
	{
		print("SCALING");

		Vector3 scale = transform.localScale;
		scale.x *= scaleFactor;
		scale.y *= scaleFactor;
		transform.localScale = scale;

		foreach (FlockTarget target in GetComponentsInChildren<FlockTarget>())
		{
			scale = target.transform.localScale;
			scale.x /= scaleFactor;
			scale.y /= scaleFactor;
			target.transform.localScale = scale;
		}
	}
}
