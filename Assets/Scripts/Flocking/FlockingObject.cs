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
		
		if ((transform.position - target.transform.position).magnitude < distToAttach * scale)
		{
			transform.parent = target.transform;
		}

		if ((Random.Range(0f, 1f) < randomCutoff) && (transform.position - target.transform.position).magnitude < distToSwitch * scale)
		{
			chooseTarget();
			transform.parent = target.transform;
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
	}
}

