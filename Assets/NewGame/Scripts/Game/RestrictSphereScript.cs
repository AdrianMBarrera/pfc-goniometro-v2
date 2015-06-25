using UnityEngine;
using System.Collections;

public class RestrictSphereScript : MonoBehaviour {

	MeshRenderer mr;
	public Transform artRest;
	
	void OnEnable(){

		GameManager.OnCalibrationPhase += SetOff;
		GameManager.OnInGamePhase += SetOn;

	}
	
	void OnDisable(){

		GameManager.OnCalibrationPhase -= SetOff;
		GameManager.OnInGamePhase -= SetOn;
	}
	
	// Use this for initialization
	void Start () {
		
		mr = GetComponent<MeshRenderer>();
		
	}
	
	void SetOff(){
		mr.enabled = false;
		
	}
	
	void SetOn(){
		mr.enabled = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("nombre" + artRest.position);
		transform.position = new Vector3(artRest.position.x, artRest.position.y, artRest.position.z);
	}
}
