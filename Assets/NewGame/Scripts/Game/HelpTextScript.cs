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
		GameManager.OnLoadGamePhase += LoadGame;
		
	}
	
	void OnDisable(){
		
		GameManager.OnLoadGamePhase -= LoadGame;
		
	}

	
	// Update is called once per frame
	void Update () {
	



		if ((GameManager.instance.stateOfGame == (int)GameManager.statesOfGame.InGame) &&
			(helpText.enabled)){

			helpText.enabled = false;
		}else

		if ((GameManager.instance.stateOfGame != (int)GameManager.statesOfGame.InGame) &&
		    (!helpText.enabled)){
			
			helpText.enabled = true;
		}

	}



	void LoadGame(){

		StartCoroutine("WaitLoad");

	}



	IEnumerator WaitLoad(){

		yield return new WaitForSeconds (2f);
		helpText.text = "Please take position";
	}




}
