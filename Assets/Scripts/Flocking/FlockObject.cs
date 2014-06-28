using UnityEngine;
using System.Collections;

public class FlockObject : MonoBehaviour
{
	float moveSpeed = 5f;
	public Sprite[] sprites;

	public FlockObject ()
	{
	}

	void Start() {
		GetComponent<SpriteRenderer> ().sprite = this.sprites[Random.Range (0, this.sprites.GetLength(0) - 1)];
	}

	void Update() {
		//getObjects ();
		if (Input.GetMouseButton(1)) {
			Vector3 pos = Input.mousePosition;
			pos.z = transform.position.z - Camera.main.transform.position.z;
			Vector3 targetPos = Camera.main.ScreenToWorldPoint(pos);
			//gameObject.transform.position = targetPos;
			gameObject.transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
		}
	}


	void getObjects() {
		FlockObject[] allObjects = FindObjectsOfType<FlockObject>();
		foreach (FlockObject other in allObjects) {
			float dist = (transform.position - other.transform.position).magnitude;
			if (dist < 30) {
				Debug.Log(other.name + ": " + dist);
			}
		}
	}
}

