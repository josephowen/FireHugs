using UnityEngine;
using System.Collections;

public class FlockTarget : MonoBehaviour
{
	public Sprite sprite;
	public GameObject flocker;

	public FlockTarget ()
	{
	}

	void Start()
	{
		GetComponent<SpriteRenderer>().sprite = this.sprite;
	}


	void Update()
	{
		if (Input.GetMouseButton(1))
		{
			Vector3 pos = Input.mousePosition;
			pos.z = transform.position.z - Camera.main.transform.position.z;
			Vector3 targetPos = Camera.main.ScreenToWorldPoint(pos);
			gameObject.transform.position = targetPos;
		}
		else if (Input.GetMouseButtonDown(0))
		{
			Vector3 pos = Input.mousePosition;
			pos.z = transform.position.z - Camera.main.transform.position.z;
			Vector3 targetPos = Camera.main.ScreenToWorldPoint(pos);
			GameObject newFlocker = (GameObject)Instantiate(flocker);
			newFlocker.transform.position = targetPos;
		}
	}
}
