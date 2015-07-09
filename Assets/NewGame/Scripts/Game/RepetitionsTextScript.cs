using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RepetitionsTextScript : MonoBehaviour {

	private Text _text;

	private int reps;

	void OnEnable(){
		GameManager.OnCalibrationPhase += SetOn;
		GameManager.OnInGamePhase += SetOn;		
		GameManager.OnDemostrationPhase += SetOff;
		GameManager.OnCheckFeedBack += IncRepetitions;
	}
	
	void OnDisable(){
		GameManager.OnCalibrationPhase -= SetOn;
		GameManager.OnInGamePhase -= SetOn;
		GameManager.OnDemostrationPhase -= SetOff;
		GameManager.OnCheckFeedBack -= IncRepetitions;
	}
	
	
	// Use this for initialization
	void Start () {
		reps = 0;
		_text = GetComponent<Text>();
		//Debug.Log ("TAMAÑO LISTA: " + InfoPlayer.alExercise.Count);
		//_text.text = "0/" + InfoPlayer.alExercise[0].InstanceRepetitions.ToString();
		_text.enabled = false;
	}
	
	
	// Update is called once per frame
	void Update () {

	}
	
	
	
	void SetOff(){
		if (InfoPlayer.gameMode != InfoPlayer.gameModes.Open)
			_text.enabled = false;	
		reps = 0;
	}
	
	
	void SetOn(){
		if (InfoPlayer.gameMode != InfoPlayer.gameModes.Open) {
			_text.enabled = true;
			_text.text = reps.ToString() + "/" + InfoPlayer.alExercise[GameManager.instance.currentExercise].InstanceRepetitions.ToString();
		}
	}

	void IncRepetitions() {
		if (InfoPlayer.gameMode != InfoPlayer.gameModes.Open) {
			reps++;
			_text.text = reps.ToString() + "/" + InfoPlayer.alExercise[GameManager.instance.currentExercise].InstanceRepetitions.ToString();
			if (reps == InfoPlayer.alExercise[GameManager.instance.currentExercise].InstanceRepetitions) {
				reps = 0;
				GameManager.instance.NextExercise();
			}
		}
	}

}
