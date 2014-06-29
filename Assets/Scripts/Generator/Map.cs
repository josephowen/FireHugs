using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
	private const float tileSize = 64.0f/100.0f;

	public string m_nextScene;
	public Material m_material;
	public int m_rows;
	public int m_columns;
	public int m_rockRow = 3;
	public int m_groundRow = 6;
	
	private GameObject prefabsContainer;
	public List<GameObject> prefabs;

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

		int elevation = m_groundRow;
		int expanse = 0;
		m_map = new Tile[m_columns][];
		for(int c = 0; c < m_map.Length; c++) {
			m_map[c] = new Tile[m_rows];
			for(int r = 0; r < m_rows; r++) {
				if(m_map[c][r] != null) {
					continue;
				}
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
						Tile.TileTypes previousType = m_map[c-1][r].Type;
						if(r == elevation) {
							// Change elevation
							if(Random.Range(0,Mathf.Max(0,10-expanse)) == 0) {
								expanse = 0;
								bool down = false;
								if(elevation <= m_groundRow) {
									elevation++;
									down = false;
								} else if (elevation >= m_rows) {
									elevation--;
									down = true;
								} else {
									down = Random.Range(0,4) == 0;
									elevation += down ? -1 : 1;
								}
								if(down) {
									Tile.TileTypes nextType = GetNextTileType(previousType, false, true);
									m_map[c][r-1] = GenerateTile(nextType, c, r-1);
									m_map[c][r] = GenerateTile(Tile.TileTypes.None, c, r);
								} else {
									Tile.TileTypes nextType = GetNextTileType(previousType, true, false);
									m_map[c][r] = GenerateTile(Tile.TileTypes.Ground, c, r);
									m_map[c][r+1] = GenerateTile(nextType, c, r+1);
								}
							}
							// Stay at current elevation
							else {
								expanse++;

								Tile.TileTypes nextType = GetNextTileType(previousType, false, false);
								m_map[c][r] = GenerateTile(nextType, c, r);

								// if we have been stable for a while, place prefabs
								prefabsContainer = prefabsContainer ?? new GameObject("Prefabs");
								if(expanse > 4) {
									Vector3 pos = m_map[c][r].transform.position;
									pos.y += tileSize;
									pos.x -= tileSize*2;
									var go = GameObject.Instantiate(prefabs[Random.Range(0,prefabs.Count)], pos, new Quaternion()) as GameObject;
									go.transform.parent = prefabsContainer.transform;
									expanse = 0;
								}
							}
						} else if (m_map[c-1][r].Type == Tile.TileTypes.Ground){
							m_map[c][r] = GenerateTile(Tile.TileTypes.Ground, c, r);
						}
						// Empty air
						else {
							m_map[c][r] = GenerateTile(Tile.TileTypes.None, c, r);
						}
					}
				}
			}
		}

		// Endzone
		var zone = new GameObject("Endzone");
		zone.transform.position = new Vector3(m_columns*tileSize, m_rows*tileSize/2,0);
		var col = zone.AddComponent<BoxCollider2D>();
		col.size = new Vector3(5*tileSize, m_rows*tileSize, 0);
		var endzone = zone.AddComponent<EndZone>();
		endzone.m_targetScene = m_nextScene;
	}

	Tile GenerateTile(Tile.TileTypes type, int row, int col) {
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
		System.Array arr = System.Enum.GetValues(typeof(Tile.TileTypes));
		int index = Random.Range(0, arr.Length-1);
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

	Tile.TileTypes GetNextTileType(Tile.TileTypes previousType, bool up, bool down) {
		if(up) {
			switch(previousType) {
				case Tile.TileTypes.GrassRight:
					return Tile.TileTypes.GrassLeftLedge;
				case Tile.TileTypes.GrassLeft:
					return Tile.TileTypes.Dirt;
				case Tile.TileTypes.Grass:
					return Tile.TileTypes.GrassLeftLedge;
				case Tile.TileTypes.Dirt:
					return Tile.TileTypes.Dirt;
				default:
					return previousType;
			}
		} else if (down) {
			switch(previousType) {
				case Tile.TileTypes.GrassRightLedge:
					return Tile.TileTypes.Grass;
				case Tile.TileTypes.GrassLeft:
					return Tile.TileTypes.Dirt;
				case Tile.TileTypes.Dirt:
					return Tile.TileTypes.Dirt;
				default:
					return previousType;
			}
		} else {
			switch(previousType) {
				case Tile.TileTypes.GrassLeftLedge:
					return Tile.TileTypes.Grass;
				case Tile.TileTypes.GrassRight:
					return Tile.TileTypes.Grass;
				case Tile.TileTypes.GrassLeft:
					return Tile.TileTypes.Dirt;
				case Tile.TileTypes.Grass:
					if(Random.Range(0,5) == 0) {
						return Tile.TileTypes.GrassLeft;
					} else {
						return Tile.TileTypes.Grass;
					}
				case Tile.TileTypes.Dirt:
					if(Random.Range(0,3) == 0) {
						return Tile.TileTypes.GrassRight;
					} else {
						return Tile.TileTypes.Dirt;
					}
				default:
					return previousType;
			}
		}
	}
}
