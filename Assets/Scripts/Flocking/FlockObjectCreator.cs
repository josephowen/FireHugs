using UnityEngine;
using System.Collections;

public class FlockObjectCreator : MonoBehaviour {

	public GameObject flocker;
	public int startObjects;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < startObjects; i++)
		{
			GameObject newFlocker = (GameObject)Instantiate(flocker);
			newFlocker.transform.position = transform.position + new Vector3(Random.Range(-10f,10f),Random.Range(-10f,10f),0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1))
		{
			Vector3 pos = Input.mousePosition;
			pos.z = transform.position.z - Camera.main.transform.position.z;
			Vector3 targetPos = Camera.main.ScreenToWorldPoint(pos);
			GameObject newFlocker = (GameObject)Instantiate(flocker);
			newFlocker.transform.position = targetPos;
		}
	}
}
