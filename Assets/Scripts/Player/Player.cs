using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float speed = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var dx = Input.GetAxis("Horizontal");
		
		transform.Translate(new Vector3(1,0,0) * speed * dx * Time.deltaTime);
		
		var dy = Input.GetAxis("Vertical");
		
		if (dy > 0 && IsGrounded()) {
			Jump();
		}
		
		updateScale();
	}
	
	void Jump() {
		rigidbody2D.AddForce(Vector3.up * 50f);
	}
	
	bool IsGrounded() {
		return Physics2D.Raycast(transform.position, -Vector3.up, 0.1f);
	}
	
	void updateScale() {
		var numObjects = gameObject.GetComponentsInChildren<FlockingObject>().Length;
		var scaleRatio = 0.05f;
		
		var scaleFactor = scaleRatio * Mathf.Sqrt(numObjects);
		
		var scale = transform.localScale;
		scale.x = scaleFactor;
		scale.y = scaleFactor;
		transform.localScale = scale;
		
		foreach (FlockTarget target in GetComponentsInChildren<FlockTarget>()) {
			scale = target.transform.localScale;
			scale.x = scaleRatio / scaleFactor;
			scale.y = scaleRatio / scaleFactor;
			target.transform.localScale = scale;
		}
	}
}
