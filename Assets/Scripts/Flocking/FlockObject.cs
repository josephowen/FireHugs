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
	public float distToAttach;
	private FlockTarget target = null;
	private FlockTarget lastTarget = null;
	public FlockSkeleton flockSkeleton = null;
	public float angularVelocity = 0f;

	void Start()
	{
		GetComponent<SpriteRenderer>().sprite = this.sprites[Random.Range(0, this.sprites.Length)];
		chooseRandomTarget();
		transform.parent = flockSkeleton.transform;
		collider2D.enabled = false;
		Destroy(rigidbody2D);
		angularVelocity = Random.Range(-10f, 10f);
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
		}

		Vector3 targetPos = target.transform.position;
		//gameObject.rigidbody2D.AddForce(moveSpeed * (targetPos - transform.position));
		transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed);
		transform.Rotate(new Vector3(0, 0, angularVelocity));
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

	//void OnTriggerStay2D(Collider2D other)
	//{
	//	if (other.transform.parent != transform.parent)
	//		return;

	//	Vector2 diff = transform.position - other.gameObject.transform.position;
	//	float magOrig = diff.magnitude;
	//	float mag = ((CircleCollider2D)other).radius * 2 - magOrig;

	//	gameObject.rigidbody2D.AddForce(avoidForce * mag * diff / magOrig);
	//}
}

