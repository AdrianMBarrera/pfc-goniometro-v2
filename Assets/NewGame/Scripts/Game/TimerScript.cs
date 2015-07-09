using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TimerScript : MonoBehaviour {

	private Text _text;

	private Image[] _imgArray;
	


	private float exerciseTime= 0f;



	
	void OnEnable(){
		GameManager.OnCalibrationPhase += SetOn;
		GameManager.OnInGamePhase += SetOn;
		GameManager.OnDemostrationPhase += CountTime;
		GameManager.OnDemostrationPhase += SetOff;
	}
	
	void OnDisable(){
		GameManager.OnCalibrationPhase -= SetOn;
		GameManager.OnInGamePhase -= SetOn;
		GameManager.OnDemostrationPhase -= SetOff;
		GameManager.OnInGamePhase -= CountTime;
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
		
		if (InfoPlayer.gameMode == InfoPlayer.gameModes.Open) {
			if (GameManager.instance.stateOfGame == GameManager.statesOfGame.InGame){
				exerciseTime += Time.deltaTime;
				GameManager.instance.TotalTime += Time.deltaTime;
				GameManager.instance.timer += Time.deltaTime;
				int minutes = (int)(GameManager.instance.TotalTime/60f);
				int seconds = (int)(GameManager.instance.TotalTime%60f);
				_text.text = string.Format("{0:00}:{1:00}", minutes ,seconds);
			}
		}
		else {
			if (GameManager.instance.stateOfGame == GameManager.statesOfGame.InGame){
				exerciseTime -= Time.deltaTime;
				GameManager.instance.TotalTime += Time.deltaTime;
				GameManager.instance.timer += Time.deltaTime;
				int minutes = (int)(exerciseTime/60f);
				int seconds = (int)(exerciseTime%60f);
				_text.text = string.Format("{0:00}:{1:00}", minutes ,seconds);
				AnyTime();
			}
		}
	}
	
	
	
	void SetOff(){
		
		foreach (Image img in _imgArray){
			img.enabled = false;
			
		}
		_text.enabled = false;

		InfoPlayer.gameModes mode = InfoPlayer.gameMode;
		
		//mode = (InfoPlayer.gameModes) 1;  //ACORDARSE DE QUITAR ESTO
		
		switch(mode){
			
		case(InfoPlayer.gameModes.Open): break;
			
			
			
		case (InfoPlayer.gameModes.Custom) :
			
//			GameManager.instance.TotalTime += ;
			break;
			
		case (InfoPlayer.gameModes.Preset) : break;
			
			
			
		}
	}
	
	
	void SetOn(){
		foreach (Image img in _imgArray){
			img.enabled = true;
			
		}
		_text.enabled =true;
	}


	void CountTime(){

		InfoPlayer.gameModes mode = InfoPlayer.gameMode;
		
		mode = InfoPlayer.gameMode;
		
		switch(mode){
			
		case(InfoPlayer.gameModes.Open):
			exerciseTime = 0;
			break;
			
		case (InfoPlayer.gameModes.Custom) :
			exerciseTime = InfoPlayer.alExercise[GameManager.instance.currentExercise].InstanceTime;
			break;
			
		case (InfoPlayer.gameModes.Preset) :
			exerciseTime = InfoPlayer.alExercise[GameManager.instance.currentExercise].InstanceTime;
			break;
			
			
		}

	}


	void AnyTime() {
		if (InfoPlayer.gameMode != InfoPlayer.gameModes.Open) {
			if (exerciseTime <= 0.1) {
				GameManager.instance.SaveStats();
				GameManager.instance.timer = 0;
				GameManager.instance.NextExercise();
			}
		}
	}



} //end class
