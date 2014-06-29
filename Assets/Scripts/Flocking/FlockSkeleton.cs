using UnityEngine;
using System.Collections;

public class FlockSkeleton : MonoBehaviour {

	public int weight = -1;

	// Use this for initialization
	void Start () {
		int totalWeight = 0;
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
