using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour {
	
	public float walkSpeed = 0.1f;
	public float runSpeed = 10000.0f;
	
	static int walkState = Animator.StringToHash("Base.Walk");
	static int angryState = Animator.StringToHash("Base.Angry");
	static int runState = Animator.StringToHash("Base.Run");
	
	private Animator anim;
	
	private GameObject player;
	
	private int walkDir = +1;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
		var animator = gameObject.GetComponent<Animator>();
		
		var distanceFromPlayer = Vector3.Distance(
				transform.position,
				player.transform.position);

		animator.SetFloat("DistanceFromPlayer", distanceFromPlayer);
		
		var runDir = transform.position.x - player.transform.position.x > 0
			? +1
			: -1;
		
		var state = GetCurrentState();
		
		if (state == walkState) {
			if (HitWall()) {
				walkDir = -walkDir;
			}
			Move(walkDir * walkSpeed);
		}
		else if (state == angryState) {
		}
		else if (state == runState) {
			if (NearWall() && IsGrounded()) {
				Jump();
			}
			Move(runDir * runSpeed);
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
	
	bool HitWall() {
		var dir = transform.localScale.x > 0 ? +1 : -1;
		return Physics2D.Raycast(
			transform.position,
			new Vector3(dir,0.1f,0),
			2.1f);
	}
	
	bool NearWall() {
		var dir = transform.localScale.x > 0 ? +1 : -1;
		return Physics2D.Raycast(
			transform.position,
			new Vector3(dir,0.1f,0),
			6f);
	}
	
	bool IsGrounded() {
		return Physics2D.Raycast(transform.position, -Vector3.up, 0.1f);
	}
	
	void Jump() {
		rigidbody2D.AddForce(Vector3.up * 20f);
	}
}
