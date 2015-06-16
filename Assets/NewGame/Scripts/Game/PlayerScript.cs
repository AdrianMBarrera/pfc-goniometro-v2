using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	SkinnedMeshRenderer mr;




	void OnEnable(){
		GameManager.OnDemostrationPhase += SetOff;
		GameManager.OnCalibrationPhase += SetOff;
		GameManager.OnInGamePhase += SetOn;
	}
	
	void OnDisable(){
		GameManager.OnInGamePhase -= SetOn;
		GameManager.OnDemostrationPhase -= SetOff;
		GameManager.OnCalibrationPhase -= SetOff;
	}
	
	// Use this for initialization
	void Start () {
		
		mr = GetComponentInChildren<SkinnedMeshRenderer>();

	}

	void SetOff(){
		mr.enabled = false;

	}

	void SetOn(){
		mr.enabled = true;

	}

	
	// Update is called once per frame
	void Update () {
		
	}

}
