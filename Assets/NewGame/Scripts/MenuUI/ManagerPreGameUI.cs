using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;


public class ManagerPreGameUI : MonoBehaviour {

	private ColorBlock _cbPressed;
	private ColorBlock _cbNormal;
	
	private Canvas _exerciseI;
	private Canvas _instanceI;
	private Canvas _planI;
	
	private Button _exerciseTab;
	private Button _instanceTab;
	private Button _planTab;

	private Text _helper; // texto de ayuda
	private LoadingScript _loading; // llamar a pasar de nivel
	private RectTransform _infoInstanceRT;
	private GameObject _infoInstancePanel;

	private Text _nameExerciseText;
	private Text _nameInstanceText;
	private Text _repetitionsText;
	private Text _timeText;

	private Animator _anim;

	private Button _playButton;

	private bool playButtonControl = false; //Controla si el boton Play es interactuable o no





	void OnEnable(){
		GameManager.OnLoadGamePhase += LoadGame;
		DummyManager.OnBeginExercise += SetStateButton;
		DummyManager.OnEndExercise += SetStateButton;
		
	}
	
	void OnDisable(){

		GameManager.OnLoadGamePhase -= LoadGame;
		DummyManager.OnBeginExercise -= SetStateButton;
		DummyManager.OnEndExercise -= SetStateButton;
	}

	// Use this for initialization
	void Start () {
		_exerciseI = GameObject.Find("InterfaceExercise").GetComponent<Canvas>();
		_instanceI = GameObject.Find("InterfaceInstance").GetComponent<Canvas>();
		_planI = GameObject.Find("InterfacePlan").GetComponent<Canvas>();

		_exerciseTab = GameObject.Find("TabExercise").GetComponent<Button>();
		_instanceTab = GameObject.Find("TabInstance").GetComponent<Button>();
		_planTab	= GameObject.Find("TabPlan").GetComponent<Button>();

		_cbNormal = _exerciseTab.colors;
		_cbPressed = _exerciseTab.colors;
		_cbPressed.normalColor = _exerciseTab.colors.pressedColor;

		_helper = GameObject.Find("TextHelp").GetComponent<Text>();
		_helper.enabled = false;
		_helper.text = "Please, Select any exercise";
		_loading = GameObject.Find("Veil").GetComponent <LoadingScript>();

		_infoInstancePanel = GameObject.Find("InfoInstance");
		_infoInstanceRT = _infoInstancePanel.GetComponent<RectTransform>();

		_nameExerciseText = GameObject.Find("ExerciseName").GetComponent<Text>();
		_nameInstanceText = GameObject.Find("InstanceName").GetComponent<Text>();
		_repetitionsText = GameObject.Find("InstanceRep").GetComponent<Text>();
		_timeText = GameObject.Find("InstanceTime").GetComponent<Text>();

		_playButton = GameObject.Find("PlayButton").GetComponent<Button>();

		_anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_exerciseI.enabled) {
			/*exerciseI.GetComponent<GameExerciseScript>().enabled = true;
			instanceI.GetComponent<GameInstanceScript>().enabled = false;
			planI.GetComponent<GamePlanScript>().enabled = false;*/
			
			_exerciseTab.colors = _cbPressed;
			_instanceTab.colors = _cbNormal;
			_planTab.colors= _cbNormal;

			_infoInstancePanel.GetComponent<Canvas>().enabled =false;

			if (!playButtonControl)
				CheckPlay();

		}
		
		if (_instanceI.enabled) {
			/*exerciseI.GetComponent<GameExerciseScript>().enabled = false;
			instanceI.GetComponent<GameInstanceScript>().enabled = true;
			planI.GetComponent<GamePlanScript>().enabled = false;*/
			
			_exerciseTab.colors = _cbNormal;
			_instanceTab.colors = _cbPressed;
			_planTab.colors= _cbNormal;
			if (_infoInstanceRT.anchorMax.x != 0.45f){

				_infoInstanceRT.anchorMin = new Vector2 (0,0.15f);
				_infoInstanceRT.anchorMax = new Vector2 (0.45f,0.35f);

				_infoInstanceRT.sizeDelta = new Vector2 (0,0);
				_infoInstanceRT.anchoredPosition = new Vector2 (0,0);

				_infoInstancePanel.GetComponent<Canvas>().enabled =true;
			}

		}
		
		if (_planI.enabled) {
			/*exerciseI.GetComponent<GameExerciseScript>().enabled = false;
			instanceI.GetComponent<GameInstanceScript>().enabled = false;
			planI.GetComponent<GamePlanScript>().enabled = true;*/
			
			_exerciseTab.colors = _cbNormal;
			_instanceTab.colors = _cbNormal;
			_planTab.colors= _cbPressed;

			if (_infoInstanceRT.anchorMax.x != 0.95f){

				_infoInstanceRT.anchorMin = new Vector2 (0.5f, 0.8f);
				_infoInstanceRT.anchorMax = new Vector2 (0.95f,0.98f);
				_infoInstanceRT.sizeDelta = new Vector2 (0,0);
				_infoInstanceRT.anchoredPosition = new Vector2 (0,0);
				_infoInstancePanel.GetComponent<Canvas>().enabled =true;
			}
			
		}
	}


	public void PlayGame (string nameLevel) {
		if (_exerciseI.enabled) {
			if (InfoPlayer.alExercise.Count < 1)
				StartCoroutine("HelpCoroutine");
			else
				_loading.BeginLevel(nameLevel);
		}
		
		/*else if (instanceI.enabled) {

		}
		
		else if (planI.enabled) {
		}*/

	}

	IEnumerator HelpCoroutine() {
		_helper.enabled = true;
		yield return new WaitForSeconds(3f);
		_helper.enabled = false;
	}


	public void CleanInfoInstancePanel() {
		_nameExerciseText.text = "Exercise: ";
		_nameInstanceText.text = "Instance: ";
		_repetitionsText.text = "Repetitions: ";
		_timeText.text = "Time: ";
	}


	//lanzamos la animacion de ocultar el canvas
	void LoadGame(){
		_anim.enabled = true;
	}


	//comprobamos si se puede pulsar el boton play

	public void CheckPlay(){

		if (InfoPlayer.alExercise.Count != 0){
			_playButton.interactable = true;

		}else{
			_playButton.interactable = false;

		}

	}


	void SetStateButton(){
		playButtonControl = !playButtonControl;
		_playButton.interactable = false;
	}




}

