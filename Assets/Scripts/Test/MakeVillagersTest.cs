using UnityEngine;
using System.Collections;

public class MakeVillagersTest : MonoBehaviour {
	
	public Transform Prefab;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Instantiate(Prefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
		}
	}
}
