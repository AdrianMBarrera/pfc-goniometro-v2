using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

	[Tooltip("Texto  ready")]
	public Text ready;
	
	private Animator readyAnim;
	
	[Tooltip("Texto con el dialogo de ayuda")]
	public Text help;
	// Use this for initialization
	void Start () {
		readyAnim = ready.gameObject.GetComponent<Animator>();


	}


	void OnEnable(){

		GameManager.OnInGamePhase += ReadyAnimation;

	}



	
	// Update is called once per frame
	void Update () {
	
		if ((GameManager.instance.stateOfGame == 0) && (!help.enabled)) {
			WaitingForCalibration();
		}
		
		if ((GameManager.instance.stateOfGame != 0) && (help.enabled)) {
			help.enabled = false;
		}


		if (GameManager.instance.stateOfGame != 2){
			readyAnim.SetBool("isIdle", true);

		}
	
	
	}


	
	void WaitingForCalibration(){
		
		help.enabled = true;
		
	}

	



	void ReadyAnimation(){
		Debug.Log("HOlaalalal");
		readyAnim.SetBool("isIdle", false);
		readyAnim.SetBool("isBig", true);
	}







}
