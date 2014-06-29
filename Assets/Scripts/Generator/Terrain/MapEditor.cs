#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor {

	private bool foldout = false;

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		Map map = (Map) target;

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

		foldout = Foldout(foldout, "Tiles", true, EditorStyles.foldout);
		if(foldout) {
			for(int i = 0; i < types.Count; i++) {
				textures[i] = (Texture2D)EditorGUILayout.ObjectField(types[i].ToString(), textures[i], typeof(Texture2D), false, null);
			}
		}

		map.Types = types;
		map.Textures = textures;
	}
	
	public static bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style)
	{
		Rect position = GUILayoutUtility.GetRect(40f, 40f, 16f, 16f, style);
		// EditorGUI.kNumberW == 40f but is internal
		return EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
	}
	
	public static bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style)
	{
		return Foldout(foldout, new GUIContent(content), toggleOnLabelClick, style);
	}
}
#endif