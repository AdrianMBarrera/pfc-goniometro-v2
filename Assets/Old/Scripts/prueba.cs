using UnityEngine;
using System.Collections;

public class prueba : MonoBehaviour {

	public MeshCollider c;
	public Mesh f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		MeshCollider mc = GetComponent<MeshCollider>();
		MeshFilter m = GetComponent<MeshFilter>();
		mc.sharedMesh = m.sharedMesh;
		Debug.Log (mc.sharedMesh.name + " " + m.sharedMesh.name);

	}
}
