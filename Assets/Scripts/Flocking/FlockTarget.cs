using UnityEngine;
using System.Collections;

public class FlockTarget : MonoBehaviour
{
	public Sprite sprite;

	public FlockTarget ()
	{
	}

	void Start()
	{
		GetComponent<SpriteRenderer>().sprite = this.sprite;

		//print (gameObject.transform.parent);
		//print(gameObject.transform.childCount);
	}


	void Update()
	{
		//if (Input.GetMouseButton(1))
		//{
		//	Vector3 pos = Input.mousePosition;
		//	pos.z = transform.position.z - Camera.main.transform.position.z;
		//	Vector3 targetPos = Camera.main.ScreenToWorldPoint(pos);
		//	gameObject.transform.position = targetPos;
		//}
	}
}
