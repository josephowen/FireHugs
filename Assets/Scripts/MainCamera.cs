using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	
	private GameObject player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		camera.transparencySortMode = TransparencySortMode.Orthographic;
	}
	
	// Update is called once per frame
	void Update () {
		// Camera follow player
		var pos = player.transform.position;
		pos.z = -50;
		pos.y += player.transform.localScale.y * 10;
		transform.position = pos;
		
		// When player grows, camera grows too
		camera.orthographicSize = player.transform.localScale.x * 20;
	}
}
