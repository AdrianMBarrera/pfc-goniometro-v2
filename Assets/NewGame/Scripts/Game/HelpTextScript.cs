using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelpTextScript : MonoBehaviour {

	Text helpText;

	// Use this for initialization
	void Start () {
	
		helpText = GetComponent<Text>();
		helpText.text = "";

	}


	void OnEnable(){
		GameManager.OnDemostrationPhase += LoadGame;
		GameManager.OnInGamePhase += DoExercise;
		GameManager.OnCalibrationPhase += Calibrate;
		
	}
	
	void OnDisable(){
		
		GameManager.OnDemostrationPhase -= LoadGame;
		GameManager.OnInGamePhase -= DoExercise;
		GameManager.OnCalibrationPhase -= Calibrate;
	}

	
	// Update is called once per frame
	void Update () {
	



		if ((GameManager.instance.stateOfGame == GameManager.statesOfGame.InGame) &&
			(helpText.enabled)){

			helpText.enabled = false;
		}else

		if ((GameManager.instance.stateOfGame != GameManager.statesOfGame.InGame) &&
		    (!helpText.enabled)){
			
			helpText.enabled = true;
		}

	}



	void LoadGame(){

		helpText.text = "Show the exercise";

	}


	void DoExercise(){
		
		helpText.text = "";
		
	}

	void Calibrate() {
		helpText.text = "Please, take position";
	}





}
