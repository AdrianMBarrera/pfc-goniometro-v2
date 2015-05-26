using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public enum statesOfGame : int {Calibration = 0 , Begin = 1, InGame =2 , End = 3 }

	[Tooltip("Temporizador del tiempo de juego")]
	public float timer;
	
	[Tooltip("estado en el que se encuentra el juego 0.- Calibrado, 1.- Empezando 2.- En Ejecucion, 3.-Final")]
	public int stateOfGame = 0;


	[Tooltip("posicion en la lista del ejercicio actual que se esta ejecutando")]
	public int currentExercise;

	//tipo de juego 






	public delegate void BeginPhase();
	public static event BeginPhase OnBeginPhase;

	public delegate void CalibrationPhase();
	public static event CalibrationPhase OnCalibrationPhase;

	public delegate void InGamePhase();
	public static event InGamePhase OnInGamePhase;
	
	public delegate void EndPhase();
	public static event EndPhase OnEndPhase;




	
	void Awake(){

		instance = this;

	}



	// Use this for initialization
	void Start () {

	

		InfoPlayer.gameModes mode = (InfoPlayer.gameModes) InfoPlayer.gameMode;

		mode = (InfoPlayer.gameModes) 1;  //ACORDARSE DE QUITAR ESTO

		switch(mode){

		case(InfoPlayer.gameModes.Open): break;


		case (InfoPlayer.gameModes.Custom) : break;


		case (InfoPlayer.gameModes.Preset) : break;



		}

	
	}
	
	// Update is called once per frame
	void Update () {




	}


	public void NextExercise(){

		if (OnInGamePhase != null){

			OnInGamePhase();

		}


	}















}
