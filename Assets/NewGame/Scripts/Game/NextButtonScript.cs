using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextButtonScript : MonoBehaviour {

	//private Animator _anim;
	
	private Image _img;
	
//	void OnEnable(){
//		GameManager.OnLoadGamePhase += LoadGame;
//		
//	}
//	
//	void OnDisable(){
//		
//		GameManager.OnLoadGamePhase -= LoadGame;
//		
//	}
	
	// Use this for initialization
	void Start () {
		
		//_anim = GetComponent<Animator>();
		
		_img = GetComponent<Image>();
		_img.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if ((GameManager.instance.stateOfGame == (int)GameManager.statesOfGame.InGame) &&
		    (_img.enabled)){
			
			_img.enabled = true;
		}else
			
			if ((GameManager.instance.stateOfGame != (int)GameManager.statesOfGame.InGame) &&
			   (!_img.enabled)){
			
			_img.enabled = false;
		}
	}
	
//	void LoadGame(){
//		
//	//	_anim.enabled = true;
//	}

}
