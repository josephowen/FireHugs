using UnityEngine;
using System.Collections;

public class SetRandomRotation : MonoBehaviour {

	void Start () {
		foreach(var renderer in GetComponentsInChildren<SpriteRenderer>()) {
			Vector3 rotation = renderer.gameObject.transform.eulerAngles;
			rotation.z = Random.Range(0, 360);
			renderer.gameObject.transform.eulerAngles = rotation;
		}
	}
}
