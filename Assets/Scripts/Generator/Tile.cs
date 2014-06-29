using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public enum TileTypes { None, Rock, Ground, Dirt, Grass, GrassLeft, GrassRight, GrassLeftLedge, GrassRightLedge };

	private Texture2D m_texture;

	public Texture2D Texture {
		get { return m_texture;}
		set { m_texture = value; }
	}

	private Material m_material;
	
	public Material Material {
		get { return m_material;}
		set { m_material = value; }
	}

	public TileTypes m_type;

	public TileTypes Type {
		get { return m_type;}
		set { m_type = value; }
	}

	public void Start() {
		if(this.Type == TileTypes.None) {
			return;
		}

		Sprite sprite = new Sprite();
		Rect rect = new Rect(0, 0, Texture.width, Texture.height);
		Vector2 center = new Vector2(0, 0);
		sprite = Sprite.Create(Texture, rect, center, 100.0f);

		var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprite;
		spriteRenderer.material = Material;

		var col = gameObject.AddComponent<BoxCollider2D>();
		col.isTrigger = false;
		col.enabled = true;
	}
}
