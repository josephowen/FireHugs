using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour {
	
	public float walkSpeed = 0.1f;
	public float runSpeed = 10000.0f;
	
	static int walkState = Animator.StringToHash("Base.Walk");
	static int angryState = Animator.StringToHash("Base.Angry");
	static int runState = Animator.StringToHash("Base.Run");
	
	private Animator anim;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
		var animator = gameObject.GetComponent<Animator>();
		
		var distanceFromPlayer = Vector3.Distance(
				transform.position,
				GameObject.Find("Player").transform.position);

		animator.SetFloat("DistanceFromPlayer", distanceFromPlayer);
		
		
		var state = GetCurrentState();
		
		if (state == walkState) {
			Move(-walkSpeed);
		}
		else if (state == angryState) {
		}
		else if (state == runState) {
			Move(+runSpeed);
		}
	}
	
	int GetCurrentState() {
		return anim.GetCurrentAnimatorStateInfo(0).nameHash;
	}
	
	void Move (float speed) {
		var scale = transform.localScale;
		if ((speed < 0 && scale.x < 0) || (speed > 0 && scale.x > 0)) {
			transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
		}
		transform.Translate(new Vector3(1,0,0) * speed * Time.deltaTime);
	}
}
