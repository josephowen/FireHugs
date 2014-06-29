using UnityEngine;
using System.Collections;

public class SetRandomSprite : MonoBehaviour {

	public Sprite[] m_sprites;

	void Start () {
		foreach(var renderer in GetComponentsInChildren<SpriteRenderer>()) {
			renderer.sprite = m_sprites[Random.Range(0, m_sprites.Length-1)];
		}
	}
}
