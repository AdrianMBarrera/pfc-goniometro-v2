﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextButtonScript : MonoBehaviour {

	//private Animator _anim;
	
	private Image _img;

	private Text _text;

	
	// Use this for initialization
	void Start () {
		
		//_anim = GetComponent<Animator>();
		
		_img = GetComponent<Image>();
		_img.enabled = false;
		_text = GetComponentInChildren <Text>();
		_text.enabled = false;
	}



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
	
	// Update is called once per frame
	void Update () {

	}



	void SetOff(){
		if (InfoPlayer.gameMode == InfoPlayer.gameModes.Open) {
			_img.enabled = false;
			_text.enabled = false;
		}
	}


	void SetOn(){
		if (InfoPlayer.gameMode == InfoPlayer.gameModes.Open) {
			_img.enabled = true;
			_text.enabled =true;
			GameManager.instance.timer = 0;

			if (GameManager.instance.currentExercise+1 < InfoPlayer.alExercise.Count) {
				_text.text = "Next >";
			}
			else {
				_text.text = "End";

				//InfoPlayer.alExercise.Clear();
				GetComponent<Button>().onClick.RemoveAllListeners();
				GetComponent<Button>().onClick.AddListener(() => {DestroyZig();});
				GetComponent<Button>().onClick.AddListener(() => {GameObject.Find("Veil").GetComponent<LoadingScript>().BeginLevel("GameStats");});
			}
		}

	}



	void DestroyZig(){
		Destroy(GameObject.Find("ZigInputContainer"));
		Destroy(GameObject.Find ("Zigfu"));
	}


}
