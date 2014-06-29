using UnityEngine;
using System.Collections;

public class Villager : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		var animator = gameObject.GetComponent<Animator>();
		
		var distanceFromPlayer = Vector3.Distance(
				transform.position,
				GameObject.Find("Player").transform.position);

		animator.SetFloat("DistanceFromPlayer", distanceFromPlayer);
	}
}
