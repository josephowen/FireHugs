using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockingObject : MonoBehaviour
{
	public float moveSpeed = 0.1f;
	public float randomCutoff = 0.04f;
	public float avoidForce = 1f;
	public float distToSwitch = 1f;
	public float distToAttach = 3f;
	public FlockTarget target = null;
	public FlockSkeleton flockSkeleton = null;
	public float angularVelocity = 0f;
	
	private GameObject player;

	void Start() {
		player = GameObject.Find("Player");
		flockSkeleton = player.GetComponentInChildren<FlockSkeleton>();
		transform.parent = flockSkeleton.transform;
		chooseRandomTarget();
		collider2D.enabled = false;
		angularVelocity = Random.Range(-240f, 240f);
		Destroy(rigidbody2D);
	}

	void Update()
	{
		var scale = player.transform.localScale.x;
		
		// When detached from parent, find new target automatically
		if (transform.parent == null) {
			chooseClosestTarget();
			chooseTarget();
			// TODO: Make object zoom to new target.
		}
		
		if ((transform.position - target.transform.position).magnitude < distToAttach * scale)
		{
			transform.parent = target.transform;
		}
		
		if (Random.Range(0f, 1f) < randomCutoff && (transform.position - target.transform.position).magnitude < distToSwitch * scale)
		{
			chooseTarget();
		}
		
		Vector3 targetPos = target.transform.position;
		transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * scale);
		transform.Rotate(new Vector3(0, 0, angularVelocity * Time.deltaTime));
	}

	void chooseRandomTarget()
	{
		FlockTarget[] allTargets = flockSkeleton.transform.parent.GetComponentsInChildren<FlockTarget>();
		target = allTargets[Random.Range(0, allTargets.Length)];
		transform.parent = target.transform;
	}

	void chooseTarget()
	{
		int randomDecision = Random.Range(0, target.GetComponent<FlockTarget>().nextOptions.Length);
		target = target.GetComponent<FlockTarget>().nextOptions[randomDecision];
		transform.parent = target.transform;
	}
	
	void chooseClosestTarget()
	{
		var allTargets = flockSkeleton.transform.parent.GetComponentsInChildren<FlockTarget>();
		FlockTarget closestTarget = null;
		var closestDistance = Mathf.Infinity;
		foreach (var currentTarget in allTargets) {
			var currentDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
			if (currentDistance < closestDistance) {
				closestDistance = currentDistance;
				closestTarget = currentTarget;
			}
		}
		if (closestTarget != null) {
			target = closestTarget;
		}
		transform.parent = target.transform;
	}
}
