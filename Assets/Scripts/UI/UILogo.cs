using UnityEngine;
using System.Collections;

public class UILogo : MonoBehaviour {

	public Texture m_texture;
	public Vector2 m_targetPosition;

	void Start () {
		renderer.material.SetTexture(0, m_texture);
		StartCoroutine(Move());
	}

	IEnumerator Move() {
		float totalTime = 5;
		float time = 0;
		while(time < totalTime) {
			yield return new WaitForEndOfFrame();
			time += Time.deltaTime;
			float ratio = time/totalTime;
			this.transform.position = Vector3.Lerp(this.transform.position, m_targetPosition, Mathf.Sqrt(ratio));
		}
		this.transform.position = m_targetPosition;
	}
}
