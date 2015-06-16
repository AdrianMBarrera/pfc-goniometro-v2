using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBarScript : MonoBehaviour {

	private Text _text;
	
	private Image[] _imgArray;
	


	void OnEnable(){
		GameManager.OnCalibrationPhase += SetOn;
		GameManager.OnInGamePhase += SetOn;

		GameManager.OnDemostrationPhase += SetOff;
	}
	
	void OnDisable(){
		GameManager.OnCalibrationPhase -= SetOn;
		GameManager.OnInGamePhase -= SetOn;
		GameManager.OnDemostrationPhase -= SetOff;

	}
	
	
	// Use this for initialization
	void Start () {
		
		//_anim = GetComponent<Animator>();
		
		_imgArray = GetComponentsInChildren<Image>();
		
		foreach (Image img in _imgArray){
			img.enabled = false;
			
		}
		_text = GetComponentInChildren <Text>();
		_text.enabled = false;
	}
	
	
	// Update is called once per frame
	void Update () {
		
		
		if (GameManager.instance.stateOfGame == GameManager.statesOfGame.InGame){

			
			//aumentar o disminuir la barra cambiar el valor del numero
			
			
		}
	}
	
	
	
	void SetOff(){
		
		foreach (Image img in _imgArray){
			img.enabled = false;
			
		}
		_text.enabled = false;
		
		InfoPlayer.gameModes mode = (InfoPlayer.gameModes) GameManager.instance.gameMode;
		

	}
	
	
	void SetOn(){
		foreach (Image img in _imgArray){
			img.enabled = true;
			
		}
		_text.enabled =true;
	}
	
	


}
