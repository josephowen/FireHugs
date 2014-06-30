using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockingObject : MonoBehaviour
{
	public float moveSpeed = 0.5f;
	public float randomCutoff = 0.04f;
	public float avoidForce = 1f;
	public float distToSwitch = 1f;
	public float distToAttach = 3f;
	public FlockTarget target = null;
	public FlockSkeleton flockSkeleton = null;

	void Start() {
		flockSkeleton = GameObject.Find("Player").GetComponentInChildren<FlockSkeleton>();
		transform.parent = flockSkeleton.transform;
		chooseRandomTarget();
		collider2D.enabled = false;
		rigidbody2D.gravityScale = 0f;
		rigidbody2D.drag = 2f;
	}

	void Update()
	{
		if ((transform.position - target.transform.position).magnitude < distToAttach)
		{
			transform.parent = target.transform;
		}

		if ((Random.Range(0f, 1f) < randomCutoff) && (transform.position - target.transform.position).magnitude < distToSwitch)
		{
			chooseTarget();
			transform.parent = target.transform;
			gameObject.rigidbody2D.AddTorque(Random.Range(-20f,20f));
		}

		Vector3 targetPos = target.transform.position;
		gameObject.rigidbody2D.AddForce(moveSpeed*(targetPos - transform.position));
	}

	void chooseRandomTarget()
	{
		FlockTarget[] allTargets = flockSkeleton.transform.parent.GetComponentsInChildren<FlockTarget>();
		//print(allTargets.Length);
		target = allTargets[Random.Range(0, allTargets.Length)];
		transform.parent = target.transform;
	}

	void chooseTarget()
	{
		int randomDecision = Random.Range(0, target.GetComponent<FlockTarget>().nextOptions.Length);
		target = target.GetComponent<FlockTarget>().nextOptions[randomDecision];
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

