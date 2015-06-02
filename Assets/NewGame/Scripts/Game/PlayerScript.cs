using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	SkinnedMeshRenderer mr;
	
	
	void OnEnable(){
		GameManager.OnLoadGamePhase += LoadGame;
		
	}
	
	void OnDisable(){
		
		GameManager.OnLoadGamePhase -= LoadGame;
		
	}
	
	
	void LoadGame(){
		mr.enabled = true;
	}
	
	// Use this for initialization
	void Start () {
		
		mr = GetComponentInChildren<SkinnedMeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
