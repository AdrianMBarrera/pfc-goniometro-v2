using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	private Animator _anim;



	void OnEnable(){
		GameManager.OnLoadGamePhase += LoadGame;
	
	}
	
	void OnDisable(){
		
		GameManager.OnLoadGamePhase -= LoadGame;
		
	}

	// Use this for initialization
	void Start () {
	
		_anim = GetComponent<Animator>();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadGame(){
		
		_anim.enabled = true;
	}


}
