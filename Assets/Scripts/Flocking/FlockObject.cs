using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockObject : MonoBehaviour
{
	public float moveSpeed = 0.5f;
	public Sprite[] sprites;
	public float randomCutoff;
	public float avoidForce;
	public float distToSwitch;
	public GameObject target = null;
	public FlockSkeleton flockSkeleton = null;

	void Start() {
		GetComponent<SpriteRenderer> ().sprite = this.sprites[Random.Range (0, this.sprites.Length)];
		chooseRandomTarget();
		transform.parent = target.transform;
	}

	void Update()
	{
		if ((Random.Range(0f, 1f) < randomCutoff) && (transform.position - target.transform.position).magnitude < distToSwitch)
		{
			chooseTarget();
			transform.parent = target.transform;
			gameObject.rigidbody2D.AddTorque(Random.Range(-20f,20f));
		}

		Vector3 targetPos = target.transform.position;
		gameObject.rigidbody2D.AddForce(moveSpeed*(targetPos - transform.position));

		//if (Input.GetMouseButton(1))
		//{
		//	Vector3 pos = Input.mousePosition;
		//	pos.z = transform.position.z - Camera.main.transform.position.z;
		//	Vector3 targetPos = Camera.main.ScreenToWorldPoint(pos);
		//	//gameObject.transform.position = targetPos;
		//	gameObject.rigidbody2D.AddForce((targetPos - transform.position));
		//	//gameObject.transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
		//}
	}

	void chooseRandomTarget()
	{
		FlockTarget[] allTargets = flockSkeleton.transform.parent.GetComponentsInChildren<FlockTarget>();
		//print(allTargets.Length);
		target = allTargets[Random.Range(0, allTargets.Length)].gameObject;
		transform.parent = target.transform;
	}

	void chooseTarget()
	{
		//print("Choosing target");
		List<GameObject> allTargets = new List<GameObject>();
		foreach (Transform child in target.transform.parent)
		{
			//print("Child found: " + child.gameObject);
			foreach (Transform childTargetTransform in child.transform)
			{
				FlockTarget childTarget = childTargetTransform.GetComponent<FlockTarget>();
				if (childTarget != null)
				{
					//print("Target under child found: " + childTarget.gameObject);
					allTargets.Add(childTarget.gameObject);
				}
			}
		}

		if (target.transform.parent.parent != null)
		{
			//print("Parent found");
			foreach (Transform parentTargetTransform in target.transform.parent.parent.transform)
			{
				FlockTarget parentTarget = parentTargetTransform.GetComponent<FlockTarget>();
				if (parentTarget != null)
				{
					//print("Target under parent found");
					allTargets.Add(parentTarget.gameObject);
				}
			}
		}

		allTargets.Add(target);
		
		int randomDecision = Random.Range(0, allTargets.Count);
		target = allTargets[randomDecision];
	}

	void chooseTargetBasic()
	{
		//print("Choosing target");
		List<GameObject> allTargets = new List<GameObject>();
		foreach (Transform child in target.transform)
		{
			allTargets.Add(child.gameObject);
		}

		if (target.transform.parent.GetComponent<FlockTarget>() != null)
		{
			allTargets.Add(target.transform.parent.gameObject);
		}

		allTargets.Add(target);

		int randomDecision = Random.Range(0, allTargets.Count);
		target = allTargets[randomDecision];
	}

	void chooseTargetWeightedOld()
	{
		//print("Choosing target");
		List<GameObject> allTargets = new List<GameObject>();
		List<int> weights = new List<int>();
		int totalWeights = 0;
		foreach (Transform child in target.transform)
		{
			allTargets.Add(child.gameObject);
			int thisWeight = child.gameObject.GetComponent<FlockTarget>().weight;
			weights.Add(thisWeight);
			totalWeights += thisWeight;
		}

		totalWeights += 1;

		if (target.transform.parent.GetComponent<FlockTarget>() != null)
		{
			allTargets.Add(target.transform.parent.gameObject);
			weights.Add(flockSkeleton.weight - totalWeights);
			totalWeights += flockSkeleton.weight;
		}

		allTargets.Add(target);
		weights.Add(1);


		//int randWeight = Random.Range(0, totalWeights);
		//int weightsSoFar = 0;
		//for (int i=0; i<allTargets.Count; i++)
		//{
		//	weightsSoFar += weights[i];
		//	if (weightsSoFar > randWeight)
		//	{
		//		target = allTargets[i];
		//		return;
		//	}
		//}

		int randomDecision = Random.Range(0, allTargets.Count);
		target = allTargets[randomDecision];
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.transform.parent != transform.parent)
			return;

		Vector2 diff = transform.position - other.gameObject.transform.position;
		float magOrig = diff.magnitude;
		float mag = ((CircleCollider2D)other).radius * 2 - magOrig;

		gameObject.rigidbody2D.AddForce(avoidForce * mag * diff/magOrig);
	}
}

