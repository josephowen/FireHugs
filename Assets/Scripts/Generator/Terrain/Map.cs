using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

	public Material m_material;
	public int m_rows;
	public int m_columns;
	public int m_rockRow = 3;
	public int m_groundRow = 6;

	private Dictionary<Tile.TileTypes, Texture2D> m_textureDictionary;

	[SerializeField]
	private List<Tile.TileTypes> m_types;
	public List<Tile.TileTypes> Types {
		get { return m_types; }
		set { m_types = value; }
	}
	
	[SerializeField]
	private List<Texture2D> m_textures;
	public List<Texture2D> Textures {
		get { return m_textures; }
		set { m_textures = value; }
	}

	private Tile[][] m_map;
	private GameObject[] m_gameObjectRow;

	void Awake() {
		// Create the texture dictionary (required because unity doesn't serialize dictionaries)
		m_textureDictionary = new Dictionary<Tile.TileTypes,Texture2D>();
		for(int i = 0; i < m_types.Count; i++) {
			m_textureDictionary.Add(m_types[i], m_textures[i]);
		}
	}

	void Start () {
		m_gameObjectRow = new GameObject[m_columns];
		for(int c = 0; c < m_gameObjectRow.Length; c++) {
			var go = new GameObject();
			go.transform.parent = this.transform;
			go.name = "Col" + c;
			m_gameObjectRow[c] = go;
		}

		m_map = new Tile[m_columns][];
		for(int c = 0; c < m_map.Length; c++) {
			m_map[c] = new Tile[m_rows];
			for(int r = 0; r < m_rows; r++) {
				// Generate baseline
				if(r < m_rockRow) {
					m_map[c][r] = GenerateTile(Tile.TileTypes.Rock, c, r);
				}
				else if(r < m_groundRow) {
					m_map[c][r] = GenerateTile(Tile.TileTypes.Ground, c, r);
				}
				// Anything above baseline
				else {
					// Starting platform
					if(c < 5) {
						if(r < m_groundRow+1) {
							m_map[c][r] = GenerateTile(Tile.TileTypes.Dirt, c, r);
						} else {
							m_map[c][r] = GenerateTile(Tile.TileTypes.None, c, r);
						}
					} else {
						if(IsValidFloorType(m_map[c-1][r].Type)) {
							int dir = UnityEngine.Random.Range(-1, 1);
							// Hills going down
							if(dir==-1 && IsValidFloorType(m_map[c-1][Math.Min(r+1,m_rows-1)].Type) && r-1 >= m_groundRow) {
								m_map[c][r] = GenerateTile(Tile.TileTypes.Grass, c, r);
								m_map[c-1][r+1].Type = Tile.TileTypes.GrassRightLedge;
							}
							// Hills going up
							else if(dir==1 && IsValidFloorType(m_map[c-1][Math.Max(0,r-1)].Type)) {
								m_map[c][r] = GenerateTile(Tile.TileTypes.Grass, c, r);
								m_map[c][r] = GenerateTile(Tile.TileTypes.GrassLeftLedge, c, r);
							}
							// Same elevation
							else {
								m_map[c][r] = GenerateTile(Tile.TileTypes.Grass, c, r);
							}
						}
						// Empty air
						else {
							m_map[c][r] = GenerateTile(Tile.TileTypes.None, c, r);
						}
					}
				}
			}
		}
	}

	Tile GenerateTile(Tile.TileTypes type, int row, int col) {
		const float tileSize = 64.0f/100.0f;
		var go = new GameObject();
		var tile = go.AddComponent<Tile>();

		tile.Type = type;
		tile.Texture = m_textureDictionary[tile.Type];
		tile.Material = m_material;
		m_map[row][col] = tile;

		go.transform.parent = m_gameObjectRow[row].transform;
		go.transform.position = new Vector2(row*tileSize, col*tileSize);
		go.name = "Row"+col;

		return tile;
	}

	Tile.TileTypes GetRandomType() {
		Array arr = Enum.GetValues(typeof(Tile.TileTypes));
		int index = UnityEngine.Random.Range(0, arr.Length-1);
		return ((Tile.TileTypes[])arr)[index];
	}

	bool IsValidFloorType(Tile.TileTypes type) {
		if(type == Tile.TileTypes.None) {
			return false;
		}
		else if(type == Tile.TileTypes.Rock) {
			return false;
		}
		else if(type == Tile.TileTypes.Ground) {
			return false;
		}
		return true;
	}
}
