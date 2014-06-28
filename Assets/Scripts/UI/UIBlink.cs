using UnityEngine;
using System.Collections;

public class UIBlink : MonoBehaviour {
	
	public float m_time = 2;
	private float m_elapsed = 0;

	void Update () {
		m_elapsed += Time.deltaTime;
		while(m_elapsed > m_time) {
			m_elapsed -= m_time;
		}
		renderer.material.color = new Color(1, 1, 1, Mathf.Abs(0.5f-(m_elapsed / m_time))*2);
	}
}
