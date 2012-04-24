using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateMeshCollider : ScriptableObject {
	[MenuItem ("FrameworkUnity/Colliders/AddMeshCollider")]
    static void AddMeshCollider() {
	    foreach (Transform transform in Selection.transforms) {
			MeshFilter[] meshs = transform.GetComponentsInChildren<MeshFilter>();
			foreach (MeshFilter mesh in meshs) {
				mesh.gameObject.AddComponent<MeshCollider>();
				mesh.GetComponent<MeshCollider>().sharedMesh = mesh.sharedMesh;
			}
	    }
    }
	
	[MenuItem ("FrameworkUnity/Colliders/DeleteMeshCollider")]
    static void DeleteMeshCollider() {
	    foreach (Transform transform in Selection.transforms) {
			MeshCollider[] colliders = transform.GetComponentsInChildren<MeshCollider>();
			foreach (MeshCollider col in colliders) {
				DestroyImmediate(col);
			}
	    }
    }
	
	[MenuItem ("FrameworkUnity/Colliders/AddBoxCollider")]
    static void AddBoxCollider() {
	    foreach (Transform transform in Selection.transforms) {
			MeshFilter[] meshs = transform.GetComponentsInChildren<MeshFilter>();
			foreach (MeshFilter mesh in meshs) {
				mesh.gameObject.AddComponent<BoxCollider>();
			}
	    }
    }
	
	[MenuItem ("FrameworkUnity/Colliders/DeleteBoxCollider")]
    static void DeleteBoxCollider() {
	    foreach (Transform transform in Selection.transforms) {
			BoxCollider[] colliders = transform.GetComponentsInChildren<BoxCollider>();
			foreach (BoxCollider col in colliders) {
				DestroyImmediate(col);
			}
	    }
    }
}
