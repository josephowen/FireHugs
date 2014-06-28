#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {

	public override void OnInspectorGUI() {
		Map map = (Map) target;
		
		map.m_rows = EditorGUILayout.IntField("Rows", map.m_rows);
		map.m_columns = EditorGUILayout.IntField("Columns", map.m_columns);
		map.m_rockRow = EditorGUILayout.IntField("Rock Row", map.m_rockRow);
		map.m_groundRow = EditorGUILayout.IntField("Dirt Row", map.m_groundRow);
		map.m_material = (Material)EditorGUILayout.ObjectField("Material", map.m_material, typeof(Material), false, null);

		List<Tile.TileTypes> types = map.Types;
		List<Texture2D> textures = map.Textures;

		if(types == null || types.Count != Enum.GetValues(typeof(Tile.TileTypes)).Length) {
			types = new List<Tile.TileTypes>();
			foreach(Tile.TileTypes type in Enum.GetValues(typeof(Tile.TileTypes))) {
				types.Add(type);
			}

			textures = new List<Texture2D>();
			for(int i = 0; i < types.Count; i++) {
				textures.Add(null);
			}
		}
		
		for(int i = 0; i < types.Count; i++) {
			textures[i] = (Texture2D)EditorGUILayout.ObjectField(types[i].ToString(), textures[i], typeof(Texture2D), false, null);
		}

		map.Types = types;
		map.Textures = textures;
	}
}
#endif