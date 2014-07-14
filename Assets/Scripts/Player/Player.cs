using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float baseSpeed = 60f;

	private float facingDir = +1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var dx = Input.GetAxis("Horizontal");
		
		Move(dx * baseSpeed * transform.localScale.x);
		
		var dy = Input.GetAxis("Vertical");
		
		if (dy > 0 && IsGrounded()) {
			Jump();
		}
		
		updateScale();
	}
	
	void Jump() {
		rigidbody2D.AddForce(Vector3.up * 140f * rigidbody2D.mass);
	}
	
	bool IsGrounded() {
		return Physics2D.Raycast(transform.position, -Vector3.up, 0.1f);
	}
	
	void updateScale() {

		var numObjects = FindObjectsOfType<FlockingObject>().Length;

		var baseScale = 0.015f;
		var scaleRatio = 0.02f;
		
		var targetScale = baseScale + scaleRatio * Mathf.Sqrt(numObjects);
		
		var scaleFactor = transform.localScale.x + (targetScale - transform.localScale.x) * 0.1f;

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
		
		rigidbody2D.mass = numObjects * 1f;
	}
	
	void Move (float dx) {
		Face(dx);
		transform.Translate(new Vector3(1,0,0) * Mathf.Abs(dx) * Time.deltaTime);
	}
	
	void Face(float dir) {
		if (dir * facingDir < 0) {
			facingDir = dir;

			foreach (var flocker in GetComponentsInChildren<FlockingObject>()) {
				flocker.transform.parent = null;
			}
			
			if (dir > 0) {
				transform.localEulerAngles = new Vector3(0,0,0);
			}
			else {
				transform.localEulerAngles = new Vector3(0,180,0);
			}
		}
	}
}
