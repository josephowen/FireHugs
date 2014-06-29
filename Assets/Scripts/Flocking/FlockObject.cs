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
	}

	void Update()
	{
		if ((Random.Range(0f, 1f) < randomCutoff) && (transform.position - target.transform.position).magnitude < distToSwitch)
		{
			chooseTarget();
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

	void chooseTarget()
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

		if (target.transform.parent != null)
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
		//print(other.gameObject.transform.position);
		float magnitude = ((CircleCollider2D)other).radius*2 - (transform.position - other.gameObject.transform.position).magnitude;
		//print("Original Magnitude: " + magnitude);
		Vector2 forceToAdd = (transform.position - other.gameObject.transform.position);
		forceToAdd = forceToAdd / forceToAdd.magnitude;
		//print("ForceToAdd: " + forceToAdd);
		//print("Magnitude (should be 1): " + forceToAdd.magnitude);
		gameObject.rigidbody2D.AddForce(avoidForce*magnitude * forceToAdd);
	}
}

