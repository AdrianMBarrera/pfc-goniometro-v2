using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class StartPositionScript : MonoBehaviour {

	private Exercise exercise; //Ejercicio que estamos editando sacado del manager exercise
	private Text initArt; //Componenete texto de la articulacion inicial
	private Text finalArt; //Componenete texto de la articulacion final
	private Text x; //Grados en x 
	private Text y; // Grados en y
	private Text z; // Grados en z
	private RotateSphere sphereScript; //Script de la esfera
	private Toggle referenceToggle; // Toogle que añade una articulacion de referencia
	private Text referenceArt;
	private Text xRef;
	private Text yRef;
	private Text zRef;
	private bool selectInitial = false; // variable para controlar la seleccion de la articulacion inicial de restriccion
	private bool selectFinal = false; // variable para controlar la seleccion de la articulacion final de restriccion
	private float lastClick = 0.0f;
	private float catchTime = 1.0f;
	private Material wood;
	private GameObject plane;
	private Vector3 ini, rotIni;

//	private float varMax = 0.0f;


	// Use this for initialization
	void Start () {
		Debug.Log ("StartPos Start");
		wood = GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().wood;
		sphereScript = GameObject.Find ("Esfera_Movimiento").GetComponent<RotateSphere>();
		exercise = GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise;
		initArt = GameObject.Find("InitialArticulation").GetComponent<Text>();
		finalArt = GameObject.Find("FinalArticulation").GetComponent<Text>();
		x = GameObject.Find("X").GetComponent<Text>();
		y = GameObject.Find("Y").GetComponent<Text>();
		z = GameObject.Find("Z").GetComponent<Text>();
		referenceArt = GameObject.Find("ReferenceArticulation").GetComponent<Text>();
		xRef = GameObject.Find("XRef").GetComponent<Text>();
		yRef = GameObject.Find("YRef").GetComponent<Text>();
		zRef = GameObject.Find("ZRef").GetComponent<Text>();
		referenceToggle = GameObject.Find("ReferenceToggle").GetComponent<Toggle>();
		ResetStart();

	}
	
	// Update is called once per frame
	void Update () {
		sphereScript.Step = 1;
		if (Input.GetMouseButtonUp(0)) {
			artSelection();
		}
		ShowInterface();
	}



	public void artSelection() {
	
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		string nameInitialArt = exercise.initialArt;
		string nameFinalArt = exercise.finalArt;
		Color iniColor = Color.green;
		Color finalColor = Color.blue;

		
		if ((Physics.Raycast(ray, out hit)) && ((Time.time-lastClick) > catchTime)) {
			
			if ((!selectInitial) && (!selectFinal)) {
				nameInitialArt = hit.collider.gameObject.name;
				hit.collider.gameObject.renderer.material.color = iniColor;
				selectInitial = true;
				initArt.text += nameInitialArt; // poner en la gui art inicial
			}
			else if ((selectInitial) && (!selectFinal) && (hit.collider.gameObject.name.Equals(nameInitialArt))) {
				nameInitialArt = "";
				hit.collider.gameObject.renderer.material = wood;
				selectInitial = false;
				initArt.text = "Initial articulation: ";
			}
			else if ((selectInitial) && (!selectFinal) && (!hit.collider.gameObject.name.Equals(nameInitialArt))) {
				nameFinalArt = hit.collider.gameObject.name;
				hit.collider.gameObject.renderer.material.color = finalColor;
				selectFinal = true;
				finalArt.text += nameFinalArt; // poner en la gui art inicial
				sphereScript.Art = nameInitialArt; // Articulacion que tiene que rotar la esfera 
			}
			else if ((selectInitial) && (selectFinal) && (hit.collider.gameObject.name.Equals(nameFinalArt))) {
				nameFinalArt = "";
				hit.collider.gameObject.renderer.material = wood;
				selectFinal = false;
				finalArt.text = "Final articulation: ";
			}
		}
	
		exercise.initialArt = nameInitialArt;
		exercise.finalArt = nameFinalArt;
		
		lastClick = Time.time;
	}


	void ShowInterface(){
		if (exercise.finalArt.CompareTo("") != 0) {

			ini = GameObject.Find(exercise.finalArt).transform.position 
				- GameObject.Find(exercise.initialArt).transform.position;

			rotIni = GameObject.Find(exercise.initialArt).transform.eulerAngles;

			//exercise.ang.Min = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString();
			//exercise.ang.Max = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString();
			exercise.ang.Min = "0";
			exercise.ang.Max = "0";

			initArt.text = "Initial articulation: "+exercise.initialArt;
			finalArt.text = "Final articulation: "+exercise.finalArt;
			// Muestra el vector normal (fuera de la ejecucion)

			// Mostrar plano
			plane = GameObject.Find("Plane");
			plane.transform.position = GameObject.Find(exercise.initialArt).transform.position;
			plane.transform.rotation = GameObject.Find(exercise.initialArt).transform.rotation;
			plane.renderer.material.shader = Shader.Find("Transparent/VertexLit");
			plane.renderer.material.color = new Color(1, 1, 1, 0.7f);
			plane.renderer.enabled = true;

			x.text = "X: " +  Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString();
			y.text = "Y: " +  Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.y).ToString();
			z.text = "Z: " + Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.z).ToString();

		}
		else {
			exercise.ini.x = "0";
			exercise.ini.y = "0";
			exercise.ini.z = "0";
		}
		/* ART DE REFERENCIA */
		if (referenceToggle.isOn) {
			
			if (!exercise.reference.nameId.Equals("")) {
				referenceArt.text = "Reference articulation: "+exercise.reference.nameId;
				xRef.text = "X: "+ Mathf.Round(GameObject.Find(exercise.reference.nameId).transform.rotation.eulerAngles.x).ToString();
				yRef.text = "Y: "+Mathf.Round(GameObject.Find(exercise.reference.nameId).transform.rotation.eulerAngles.y).ToString();
				zRef.text  = "Z: "+Mathf.Round(GameObject.Find(exercise.reference.nameId).transform.rotation.eulerAngles.z).ToString();
			}
			
			if ((!exercise.initialArt.Equals("")) && (!exercise.finalArt.Equals("")) 
			    && (exercise.reference.nameId.Equals("")))

				if (Input.GetMouseButtonUp(0)) {
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit)) {
						referenceArt.text = "Reference articulation: " + hit.collider.gameObject.name;
						exercise.reference.nameId = hit.collider.gameObject.name;
						hit.collider.gameObject.renderer.material.color = Color.yellow;
					}
				}
			}
			else if (!exercise.reference.nameId.Equals("")){
				GameObject.Find(exercise.reference.nameId).renderer.material = wood;
				exercise.reference.nameId = "";
			}
		/* ART DE REFERENCIA */
		
		sphereScript.Art = exercise.initialArt;
		}


	//BUSCAR EL ID DE LA ART POR EL NOMBRE 

	int searchIdArt (string name) {
		switch (name) {
		case "Head":			return 1;
		case "Neck":			return 2;
		case "Chest":			return 3;
		case "Spine": 			return 4;
		case "LeftCollar": 		return 5;
		case "LeftShoulder":	return 6;
		case "LeftElbow":		return 7;
		case "LeftWrist":		return 8;
		case "LeftHand":		return 9;
		case "LeftFingertip": 	return 10;
		case "RightCollar": 	return 11;
		case "RightShoulder": 	return 12;
		case "RightElbow": 		return 13;
		case "RightWrist": 		return 14;
		case "RightHand": 		return 15;
		case "RightFingertip": 	return 16;
		case "LeftHip": 		return 17;
		case "LeftKnee": 		return 18;
		case "LeftAnkle": 		return 19;
		case "LeftFoot": 		return 20;
		case "RightHip": 		return 21;
		case "RightKnee": 		return 22;
		case "RightAnkle": 		return 23;
		case "RightFoot": 		return 24;
		default: return 0;
		}
	}



	public void PassToManager(){

		exercise.ini.x = Mathf.Round(ini[0]).ToString();
		exercise.ini.y = Mathf.Round(ini[1]).ToString();
		exercise.ini.z = Mathf.Round(ini[2]).ToString();
		
		exercise.rotIni.x = Mathf.Round(rotIni.x).ToString();
		exercise.rotIni.y = Mathf.Round(rotIni.y).ToString();
		exercise.rotIni.z = Mathf.Round(rotIni.z).ToString();




		//reference

		if (referenceToggle.isOn) {
			exercise.reference.id = searchIdArt(exercise.reference.nameId);
			exercise.reference.x = Mathf.Round(GameObject.Find(exercise.reference.nameId).transform.rotation.eulerAngles.x).ToString();
			exercise.reference.y = Mathf.Round(GameObject.Find(exercise.reference.nameId).transform.rotation.eulerAngles.y).ToString();
			exercise.reference.z = Mathf.Round(GameObject.Find(exercise.reference.nameId).transform.rotation.eulerAngles.z).ToString();

		}else if (!exercise.reference.nameId.Equals("")){
			exercise.reference.id = 0;
			exercise.reference.x = "";
			exercise.reference.y = "";
			exercise.reference.z = "";
		}


		GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise = exercise;
	}


	private void ResetStart() {

		initArt.text = "Initial articulation: ";
		finalArt.text = "Final articulation: ";
		
		x.text = "X: ";
		y.text = "Y: ";
		z.text = "Z: ";

		referenceArt.text = "Reference articulation: ";
		xRef.text = "X: ";
		yRef.text = "Y: ";
		zRef.text = "Z: ";

		GameObject.Find ("Plane").renderer.enabled = false;
	}
}




