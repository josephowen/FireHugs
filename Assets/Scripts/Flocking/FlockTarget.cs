using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockTarget : MonoBehaviour
{
	public Sprite sprite;
	public FlockTarget[] nextOptions;

	void Start()
	{
		GetComponent<SpriteRenderer>().sprite = this.sprite;

		computeNextOptions();

		//print (gameObject.transform.parent);
		//print(gameObject.transform.childCount);
	}

	void computeNextOptions()
	{
		List<FlockTarget> allTargets = new List<FlockTarget>();
		foreach (Transform child in transform.parent)
		{
			foreach (Transform childTargetTransform in child.transform)
			{
				FlockTarget childTarget = childTargetTransform.GetComponent<FlockTarget>();
				if (childTarget != null)
				{
					allTargets.Add(childTarget);
				}
			}
		}

		if (transform.parent.parent != null)
		{
			foreach (Transform parentTargetTransform in transform.parent.parent.transform)
			{
				FlockTarget parentTarget = parentTargetTransform.GetComponent<FlockTarget>();
				if (parentTarget != null)
				{
					allTargets.Add(parentTarget);
				}
			}
		}

		allTargets.Add(this);

		nextOptions = new FlockTarget[allTargets.Count];

		for (int i = 0; i < allTargets.Count; i++)
		{
			nextOptions[i] = allTargets[i];
		}
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
