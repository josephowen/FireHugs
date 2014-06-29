using UnityEngine;
using System.Collections;

public class FlockTarget : MonoBehaviour
{
	public Sprite sprite;

	public int weight = -1;

	void Start()
	{
		GetComponent<SpriteRenderer>().sprite = this.sprite;

		getWeight();

		//print (gameObject.transform.parent);
		//print(gameObject.transform.childCount);
	}

	public int getWeight()
	{
		int totalWeight = 1;
		foreach (Transform childTransform in transform)
		{
			if (childTransform.gameObject.GetComponent<FlockTarget>().weight == -1)
			{
				totalWeight += childTransform.gameObject.GetComponent<FlockTarget>().getWeight();
			}
			else
			{
				totalWeight += childTransform.gameObject.GetComponent<FlockTarget>().weight;
			}
		}

		weight = totalWeight;
		return totalWeight;
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
