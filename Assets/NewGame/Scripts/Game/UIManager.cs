using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

	[Tooltip("Texto  ready")]
	public Text ready;
	

	[Tooltip("Texto con el dialogo de ayuda")]
	public Text help;
	// Use this for initialization

	private Canvas _canvas;

	void OnEnable(){
		GameManager.OnLoadGamePhase += LoadGame;
		
	}
	
	void OnDisable(){
		
		GameManager.OnLoadGamePhase -= LoadGame;
		
	}

	void Start () {
		_canvas = GetComponent<Canvas>();
	}
// Update is called once per frame
	void Update () {

	}



	void LoadGame(){

		_canvas.enabled = true;

	}


} //endclass
