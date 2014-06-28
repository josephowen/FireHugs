#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Provides a list of Components to Game Objects in alphabetical order.
/// 
/// This is a modified version of Angry Ant's script; the original script can
/// be found here: http://angryant.com/2009/08/31/where-did-that-component-go/
/// 
/// Also utilizes foldout utility code from http://answers.unity3d.com/questions/684414/custom-editor-foldout-doesnt-unfold-when-clicking.html
/// </summary>
public class ComponentLister : EditorWindow
{
	private Hashtable m_sets = new Hashtable();
	private List<System.Type> m_types = new List<System.Type>();
	private Dictionary<System.Type, bool> m_foldouts = new Dictionary<System.Type, bool>();
	
	private Vector2 m_scrollPosition;
	
	[MenuItem("Window/Component Lister")]
	public static void Launch()
	{
		EditorWindow window = GetWindow(typeof(ComponentLister));
		window.Show();
	}
	
	public void UpdateList()
	{
		// Reset the data
		m_sets.Clear();
		m_types.Clear();
		m_foldouts.Clear();
		
		// Find all of the components in the scene
		Object[] objects = FindObjectsOfType(typeof(Component));
		foreach(Component component in objects)
		{
			System.Type type = component.GetType();
			if(!m_sets.ContainsKey(type))
			{
				m_sets[type] = new ArrayList();
				m_types.Add(type);
				m_foldouts.Add(type, false);
			}
			
			((ArrayList)m_sets[type]).Add(component.gameObject);
		}
		
		// Sort the key list
		m_types.Sort((a,b) => a.Name.CompareTo(b.Name));
	}
	
	public void OnGUI()
	{
		GUILayout.BeginHorizontal(GUI.skin.GetStyle("Box"));
			GUILayout.Label("Components in scene:");
			GUILayout.FlexibleSpace();
			
			if(GUILayout.Button("Refresh"))
			{
				UpdateList();
			}
		GUILayout.EndHorizontal();
		
		m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition);
			
			foreach(System.Type type in m_types)
			{
				m_foldouts[type] = Foldout(m_foldouts[type], type.Name, true, EditorStyles.foldout);
				if(m_foldouts[type])
				{
					foreach(GameObject gameObject in (ArrayList) m_sets[type])
					{
						if(GUILayout.Button(gameObject.name))
						{
							Selection.activeObject = gameObject;
						}
					}
				}
			}
			
		GUILayout.EndScrollView();
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
