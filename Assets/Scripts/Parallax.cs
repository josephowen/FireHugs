using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Parallax : MonoBehaviour {
	
	public Vector3 offset;
	public Sprite[] m_backgrounds;
	public int[] m_order;
	public float[] m_backgroundSpeeds;

	private GameObject m_bgContainer;
	private Dictionary<int, GameObject> m_bgDictionary;

	void Awake() {
		m_bgDictionary = new Dictionary<int,GameObject>();
		m_bgContainer = new GameObject("Background");
		m_bgContainer.transform.position = offset;

		for(int i = 0; i < m_backgrounds.Length; i++) {
			var go = new GameObject(m_backgrounds[i].name);
			go.transform.parent = m_bgContainer.transform;
			go.transform.localPosition = Vector3.zero;
			m_bgDictionary.Add(i, go);
			var sr = go.AddComponent<SpriteRenderer>();
			sr.sprite = m_backgrounds[i];
			sr.sortingOrder = m_order[i];
		}
	}
	
	void Update () {
		
	}
}
