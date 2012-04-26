using UnityEngine;
using UnityEditor;
using System.Collections;

public class WindowTagChildren : ScriptableWizard {
	
	static private string tag;
	static private GUIStyle buttonStyle, labelStyle;
	
	[MenuItem ("FrameworkUnity/Children/Insert Tag")]
    static void Init () {
	    WindowTagChildren window = (WindowTagChildren)EditorWindow.GetWindow (typeof (WindowTagChildren));
    }
    
    void OnGUI () {
		if (Selection.objects.Length != 0) {
			tag = EditorGUILayout.TagField("Tag: ", tag);
			
			GUILayout.Space(15);
			
			GUILayout.BeginHorizontal();
			
			GUI.backgroundColor = Color.green;
			
			GUILayout.Label(" ");
			
			if (GUILayout.Button("Apply")) {
				foreach (GameObject selected in Selection.gameObjects) {
					foreach (Transform child in selected.GetComponentsInChildren<Transform>()) {
						child.tag = tag;
					}
					selected.tag = tag;
				}
			}
		} else {
			GUI.contentColor = Color.red;
			GUILayout.Label("Select a Object!");
		}
    }
}
