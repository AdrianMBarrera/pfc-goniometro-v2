using UnityEngine;
using System.Collections;

public class UpdateCollider : MonoBehaviour {

	public MeshCollider mc;
	public SkinnedMeshRenderer smr;
	public MeshFilter mf;
	public Mesh meshToCollide;

	// Use this for initialization
	void Start () {
		mc = GetComponent<MeshCollider>();
		smr = GetComponent<SkinnedMeshRenderer>();
		//mf = GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update () {
		//mf = GetComponent<MeshFilter>();
	//	mc.sharedMesh = null;
		//mc.sharedMesh = mf.mesh;

	}
}
