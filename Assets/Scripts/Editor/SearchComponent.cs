using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SearchComponent : ScriptableWizard {
	public string componentName = "";
	
	private bool isToSelect;
	
	[MenuItem ("FrameworkUnity/Components/Search")]
	
	static void CreateWizard ()
	{
	    ScriptableWizard.DisplayWizard("Search Component", typeof(SearchComponent), "Search");  
	}
	
	void OnWizardCreate ()
	{
		List<Object> objs = new List<Object>();
		if (Selection.objects.Length != 0) {
			foreach (GameObject selected in Selection.gameObjects) {
				objs.Add(selected);
				foreach (Transform child in selected.GetComponentsInChildren<Transform>()) {
					objs.Add(child.gameObject);
				}
			}
			isToSelect = true;
		} else {
			Object[] finds = FindObjectsOfType(typeof(GameObject));
			foreach (Object find in finds) {
				objs.Add(find);
			}
			isToSelect = false;
		}
		if (!isToSelect) {
			Component component = new Component();
			bool haveComponent = false;
	        foreach (GameObject go in objs) {
				if (go.GetComponent(componentName) != null) {
		            component = go.GetComponent(componentName);
					haveComponent = true;
				}
	        }
			if (haveComponent) {
				SceneModeUtility.SearchForType(component.GetType());
			} else {
				Debug.LogError("The field Type Component is not exist.");
			}
		} else {
	        ArrayList selectionBuilder = new ArrayList();        
	        foreach (GameObject go in objs) {
	            if(go.GetComponent(componentName))
	                selectionBuilder.Add(go);
	        }            
	        Selection.objects = selectionBuilder.ToArray(typeof(GameObject)) as GameObject[];
		}
	}
	
}
