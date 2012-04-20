using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateMeshCollider : ScriptableObject {
	[MenuItem ("FrameworkUnity/CreateMeshCollider")]
    static void Create() {
	    foreach (Transform transform in Selection.transforms) {
			MeshFilter[] meshs = transform.GetComponentsInChildren<MeshFilter>();
			foreach (MeshFilter mesh in meshs) {
				mesh.gameObject.AddComponent<MeshCollider>();
				mesh.GetComponent<MeshCollider>().sharedMesh = mesh.sharedMesh;
			}
	    }
    }
}
