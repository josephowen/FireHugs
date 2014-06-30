using UnityEngine;
using System.Collections;

public class Unfreezer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col) {
		if(col.tag == "Player") {
			rigidbody2D.isKinematic = false;
			collider2D.isTrigger = false;
			if (gameObject.tag == "Rock") {
				gameObject.AddComponent("FlockingObject");
			}
		}
	}
}
