using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadyTextScript : MonoBehaviour {

	private Animator _readyAnim;
	private Text _t;

	void OnEnable(){
		//GameManager.OnLoadGamePhase += ReadyAnimation;
		GameManager.OnInGamePhase += ReadyAnimation;
		GameManager.OnDemostrationPhase += SetOff;
		GameManager.OnCalibrationPhase += SetOff;
	}


	void OnDisable(){
		//GameManager.OnLoadGamePhase += ReadyAnimation;
		GameManager.OnInGamePhase -= ReadyAnimation;
		GameManager.OnDemostrationPhase -= SetOff;
		GameManager.OnCalibrationPhase -= SetOff;
	}


	// Use this for initialization
	void Start () {
		_readyAnim = GetComponent<Animator>();
		
		_t = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void ReadyAnimation(){
		_t.enabled = true;
		_readyAnim.Play("Idle");
		_readyAnim.SetBool("isBig", true);
	}



	void SetOff(){

		_t.enabled = false;
	}






}
