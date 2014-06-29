using UnityEngine;
using System.Collections;

public class EndZone : MonoBehaviour {
	
	public string m_targetScene;
	
	void OnTriggerEnter2D(Collider2D col) {
		if(col.tag == "Player") {
			Application.LoadLevel(m_targetScene);
		}
	}
}
