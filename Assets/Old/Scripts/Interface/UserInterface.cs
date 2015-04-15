using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;

public class UserInterface : MonoBehaviour {
	
	public RotateSphere sphereScript; //Script de la esfera
	
	public LineRenderer line;

	public Material wood;
	
	public bool condRest = false; //Variable para saber si estamos eligiendo restricciones
	private bool condMessage = false; //Variable para mostrar mensaje durante unos segundos
	
	// En este objeto almacenaremos el ejercicio
	public Exercise exercise = new Exercise();
	public Restriction restriction = new Restriction();
	
	// En esta variable iremos almacenando el fichero
	public string file = "";

	// Variables para establecer el tiempo entre seleciones
	public float lastClick = 0.0f;
	public float catchTime = 1.0f;
	
	//Estas variables indican si las articulaciones Inicial y Final estan seleccionadas
	public bool selectInitial = false; // variable para controlar la seleccion de la articulacion inicial
	public bool selectFinal = false; // variable para controlar la seleccion de la articulacion final
	
	public Vector3 ini;
	public Vector3 fin;
	public Vector3 eje;

	public bool move = false; // no puedes mover hasta que tengas las dos articulaciones selecionadas
	public bool condRef = false; // variable para saber si hay una articulacion de referencia 


	public int toolbarInt = 0;
	public string[] toolbarStrings = new string[] {"Restrictions", "Start position", "Final position", "Save"};
	
	public GameObject plano;
	
	public float varMax = 0.0f;

	public Vector3 rotIni;
	public Vector3 rotFin;

	// Use this for initialization
	void Start () {
		plano = GameObject.CreatePrimitive(PrimitiveType.Cube);
		plano.name = "Plano";
		plano.transform.localScale = new Vector3(0.05f, 7f, 7f);
		plano.collider.enabled = false;
		plano.renderer.enabled = false;

		sphereScript = GameObject.Find ("Esfera_Movimiento").GetComponent<RotateSphere>();

		line = gameObject.AddComponent<LineRenderer>();
		line.SetWidth(0.05f, 0.05f);
		line.SetVertexCount(2);
		line.material = new Material (Shader.Find("Particles/Additive"));
		line.SetColors(Color.red, Color.red);
		line.renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Interfaz
	void OnGUI () {		
		GUI.Box(new Rect(Screen.width-Screen.width/3, 0, Screen.width/3, Screen.height),"\nMOVEMENTS INTERFACE");
		toolbarInt = GUI.Toolbar (new Rect (Screen.width - Screen.width/3 + 30, 50, 400, 25), toolbarInt, toolbarStrings);
		
		switch (toolbarInt) {
			case 0: condRest = true;
					Step1Restriction();
					break;
			case 1: condRest = false;
					Step2Start();
					break;
			case 2: condRest = false;
					Step3Final();
					break;
			case 3: condRest = false;
					Step4Save();
					break;
		}

		sphereScript.Step = toolbarInt;

		if (GUI.Button(new Rect(10, Screen.height-40, 100, 30), "Reset"))
			ResetExercise();
	}

	public void ResetExercise() {
		
		if (!exercise.finalArt.Equals(""))
			GameObject.Find(exercise.finalArt).renderer.material = wood;

		if (!exercise.initialArt.Equals("")) {
			GameObject.Find(exercise.initialArt).renderer.material = wood;
			GameObject.Find(exercise.initialArt).transform.eulerAngles = new Vector3(0,180,0);
		}

		if (!restriction.finalArt.Equals(""))
			GameObject.Find(restriction.finalArt).renderer.material = wood;
		
		if (!restriction.initialArt.Equals("")) {
			GameObject.Find(restriction.initialArt).renderer.material = wood;
			GameObject.Find(restriction.initialArt).transform.eulerAngles = new Vector3(0,180,0);
		}

		foreach (Restriction r in exercise.Restrictions) {
			GameObject.Find(r.initialArt).renderer.material = wood;
			GameObject.Find(r.finalArt).renderer.material = wood;
			GameObject.Find(r.initialArt).transform.eulerAngles = new Vector3(0,180,0);
		}

		toolbarInt = 0;

		exercise = new Exercise();
		restriction = new Restriction();

		rotIni = new Vector3(0,0,0);
		rotFin = new Vector3(0,0,0);

		selectInitial = false;
		selectFinal = false;

		plano.renderer.enabled = false;
		line.renderer.enabled = false;

		file = "";
		varMax = 0.0f;

		move = false;
		condRef = false;
		condRest = false;
//		condMessage = false;
	}


	// Paso 1
	public void Step1Restriction() {
		GUI.Label(new Rect(Screen.width-Screen.width/4+20, 100, 200, 30), "Initial articulation: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+130, 100, 200, 30), restriction.initialArt);
		
		GUI.Label(new Rect(Screen.width-Screen.width/4+20, 130, 200, 30), "Final articulation: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+130, 130, 200, 30), restriction.finalArt);
		
		GUI.Label(new Rect(Screen.width-Screen.width/4+20 , 200, 200, 30), "Rotation: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+120, 200, 200, 30), "X: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+120, 230, 200, 30), "Y: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+120, 260, 200, 30), "Z: ");
		
		GUI.Label(new Rect(Screen.width-Screen.width/4+20, 330, 200, 30), "Grades: ");
		string g = restriction.grade.ToString();
		g = GUI.TextField(new Rect(Screen.width-Screen.width/4+100, 330, 50, 20), g, 3);
		restriction.grade = int.Parse(g);
		
		artSelection();
		
		if (!restriction.finalArt.Equals("")) {

			restriction.x = (int) Mathf.Round(GameObject.Find(restriction.finalArt).transform.position.x)
				          - (int) Mathf.Round(GameObject.Find(restriction.initialArt).transform.position.x);

			restriction.y = (int) Mathf.Round(GameObject.Find(restriction.finalArt).transform.position.y)
						  - (int) Mathf.Round(GameObject.Find(restriction.initialArt).transform.position.y);

			restriction.z = (int) Mathf.Round(GameObject.Find(restriction.finalArt).transform.position.z)
				          - (int) Mathf.Round(GameObject.Find(restriction.initialArt).transform.position.z);

			restriction.rotX = (int) Mathf.Round(GameObject.Find(restriction.initialArt).transform.eulerAngles.x);
			restriction.rotY = (int) Mathf.Round(GameObject.Find(restriction.initialArt).transform.eulerAngles.y);
			restriction.rotZ = (int) Mathf.Round(GameObject.Find(restriction.initialArt).transform.eulerAngles.z);
			
			GUI.Label(new Rect(Screen.width-Screen.width/4+150, 200, 200, 30),
			          Mathf.Round(GameObject.Find(restriction.initialArt).transform.rotation.eulerAngles.x).ToString());
			GUI.Label(new Rect(Screen.width-Screen.width/4+150, 230, 200, 30),
			          Mathf.Round(GameObject.Find(restriction.initialArt).transform.rotation.eulerAngles.y).ToString());
			GUI.Label(new Rect(Screen.width-Screen.width/4+150, 260, 200, 30), 
			          Mathf.Round(GameObject.Find(restriction.initialArt).transform.rotation.eulerAngles.z).ToString());
		}
		else {
			GUI.Label(new Rect(Screen.width-Screen.width/4+150, 200, 200, 30), "0");
			GUI.Label(new Rect(Screen.width-Screen.width/4+150, 230, 200, 30), "0");
			GUI.Label(new Rect(Screen.width-Screen.width/4+150, 260, 200, 30), "0");
		}
		
		if (GUI.Button(new Rect(Screen.width-275, Screen.height-50, 125, 30), "Add restriction")) {
			move = false;
			exercise.Restrictions.Add(restriction);
			restriction = new Restriction();
			selectInitial = false;
			selectFinal = false;
		}
		
		sphereScript.Art = restriction.initialArt;
		
		//	if (GUI.Button(Rect(Screen.width-275, Screen.height-30, 125, 30), "View Restrictions")) {
		int i = 0;
		foreach (Restriction r in exercise.Restrictions) {
			GUI.Label(new Rect(Screen.width-Screen.width/4+20, 400+(i*30), 300, 30), "Restriction " + i + ": " + 
			          r.initialArt + ", " + r.finalArt);
			i++;
		}
		//	}
	}
	
	
	// Paso 2
	public void Step2Start() {
		GUI.Label(new Rect(Screen.width-Screen.width/4+20, 100, 200, 30), "Initial articulation: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+200, 100, 200, 30), exercise.initialArt);
		
		GUI.Label(new Rect(Screen.width-Screen.width/4+20, 130, 200, 30), "Final articulation: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+200, 130, 200, 30), exercise.finalArt);
		
		GUI.Label(new Rect(Screen.width-Screen.width/4+20 , 160, 200, 30), "Initial position: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+120, 160, 200, 30), "X: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+120, 190, 200, 30), "Y: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+120, 220, 200, 30), "Z: ");

		condRef = GUI.Toggle(new Rect(Screen.width-Screen.width/4+20, 250, 200, 30), condRef, "Add reference articulation");
		
		//Seleccionamos las articulaciones Inicial y Final
		artSelection();
		
		//Si ya tenemos la articulacion final seleccionada
		if (!exercise.finalArt.Equals("")) {
			
			// Muestra el vector normal (fuera de la ejecucion)
			ini = GameObject.Find(exercise.finalArt).transform.position //IMPORTANTE
				- GameObject.Find(exercise.initialArt).transform.position;
			exercise.ini.x = Mathf.Round(ini[0]).ToString();
			exercise.ini.y = Mathf.Round(ini[1]).ToString();
			exercise.ini.z = Mathf.Round(ini[2]).ToString();

			rotIni = GameObject.Find(exercise.initialArt).transform.eulerAngles;
			
			// Mostrar plano
			plano.transform.position = GameObject.Find(exercise.initialArt).transform.position;
			plano.transform.rotation = GameObject.Find(exercise.initialArt).transform.rotation;
			plano.renderer.material.shader = Shader.Find("Transparent/VertexLit");
			plano.renderer.material.color = new Color(1, 1, 1, 0.7f);
			plano.renderer.enabled = true;

			exercise.ang.Min = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString();
			varMax = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x);
			
			GUI.Label(new Rect(Screen.width-Screen.width/4+150, 160, 200, 30),
			          Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString());
			
			GUI.Label(new Rect(Screen.width-Screen.width/4+150, 190, 200, 30),
			          Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.y).ToString());
			
			GUI.Label(new Rect(Screen.width-Screen.width/4+150, 220, 200, 30),
			          Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.z).ToString());
		}
		else {
			exercise.ini.x = "0";
			exercise.ini.y = "0";
			exercise.ini.z = "0";
		}
		
		
		/* ART DE REFERENCIA */
		if (condRef) {
			GUI.Label(new Rect(Screen.width-Screen.width/4+20, 280, 200, 30), "Reference articulation: ");
			GUI.Label(new Rect(Screen.width-Screen.width/4+20 , 310, 200, 30), "Rotation: ");
			GUI.Label(new Rect(Screen.width-Screen.width/4+120, 310, 200, 30), "X: ");
			GUI.Label(new Rect(Screen.width-Screen.width/4+120, 340, 200, 30), "Y: ");
			GUI.Label(new Rect(Screen.width-Screen.width/4+120, 370, 200, 30), "Z: ");

			if (!exercise.reference.nameId.Equals("")) {
				GUI.Label(new Rect(Screen.width-Screen.width/4+200, 280, 200, 30), exercise.reference.nameId);

				GUI.Label(new Rect(Screen.width-Screen.width/4+150, 310, 200, 30),
				          Mathf.Round(GameObject.Find(exercise.reference.nameId).transform.rotation.eulerAngles.x).ToString());
				
				GUI.Label(new Rect(Screen.width-Screen.width/4+150, 340, 200, 30),
				          Mathf.Round(GameObject.Find(exercise.reference.nameId).transform.rotation.eulerAngles.y).ToString());
				
				GUI.Label(new Rect(Screen.width-Screen.width/4+150, 370, 200, 30),
				          Mathf.Round(GameObject.Find(exercise.reference.nameId).transform.rotation.eulerAngles.z).ToString());
			}

			if ((!exercise.initialArt.Equals("")) && (!exercise.finalArt.Equals("")))
				if (Input.GetMouseButtonUp(0)) {
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit)) {
						GUI.Label(new Rect(Screen.width-Screen.width/4+200, 250, 200, 30), hit.collider.gameObject.name);
						exercise.reference.nameId = hit.collider.gameObject.name;
						exercise.reference.id = searchIdArt(hit.collider.gameObject.name);
						exercise.reference.x = Mathf.Round(hit.collider.gameObject.transform.rotation.eulerAngles.x).ToString();
						exercise.reference.y = Mathf.Round(hit.collider.gameObject.transform.rotation.eulerAngles.y).ToString();
						exercise.reference.z = Mathf.Round(hit.collider.gameObject.transform.rotation.eulerAngles.z).ToString();
						hit.collider.gameObject.renderer.material.color = Color.yellow;
					}
				}
		}
		else if (!exercise.reference.nameId.Equals("")){
			GameObject.Find(exercise.reference.nameId).renderer.material = wood;
			exercise.reference.nameId = "";
			exercise.reference.id = 0;
			exercise.reference.x = "";
			exercise.reference.y = "";
			exercise.reference.z = "";
		}
		/* ART DE REFERENCIA */
		
		sphereScript.Art = exercise.initialArt;
	}
	
	
	// Paso 3
	public void Step3Final() {
		GUI.Label(new Rect(Screen.width-Screen.width/4+20, 100, 200, 30), "Rotation: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+100, 100, 200, 30), "Min: ");
		GUI.Label(new Rect(Screen.width-Screen.width/4+100, 130, 200, 30), "Max: ");
		moveArt();
		
		fin = GameObject.Find(exercise.finalArt).transform.position - GameObject.Find(exercise.initialArt).transform.position;
		
		var x = ini[1]*fin[2] - ini[2]*fin[1];
		var y = ini[2]*fin[0] - ini[0]*fin[2];
		var z = ini[0]*fin[1] - ini[1]*fin[0];

		Debug.Log("ini " + ini[0]+ " "+ ini[1] + "" + ini[2]);
		Debug.Log("fin " + fin[0]+ " "+ fin[1] + "" + fin[2]);

		eje = new Vector3(x,y,z);

		
		line.renderer.enabled = true;
		line.SetPosition(0, GameObject.Find(exercise.initialArt).transform.position);
		line.SetPosition(1, GameObject.Find(exercise.initialArt).transform.position-eje);
		
		exercise.axis.X = Mathf.Round(x).ToString();
		exercise.axis.Y = Mathf.Round(y).ToString();
		exercise.axis.Z = Mathf.Round(z).ToString();

		exercise.ang.Max = Mathf.Round(varMax).ToString();
		
		GUI.Label(new Rect(Screen.width-Screen.width/4+160, 100, 200, 30), exercise.ang.Min);
		GUI.Label(new Rect(Screen.width-Screen.width/4+160, 130, 200, 30), exercise.ang.Max);
		
		moveArt();

		rotFin = GameObject.Find(exercise.initialArt).transform.eulerAngles;
	}
	
	
	// Paso 4 - Guardar fichero
	public void Step4Save() {

		if (GUI.Button(new Rect(Screen.width-Screen.width/3 + 30, 120, 100, 30), "Show"))
			ShowMovement();

		GUI.Label(new Rect(Screen.width-Screen.width/3 + 30, 300, 200, 30), "Name file: ");
		file = GUI.TextField(new Rect(Screen.width-Screen.width/3 + 100, 300, 150, 20), file);
		
		if (GUI.Button(new Rect(Screen.width-125, Screen.height-50, 100, 30), "Guardar")) {
//			string xmlPath = (Application.dataPath) + "/Results";
			string xmlPath = "./Exercises";

			if (!Directory.Exists(xmlPath))
				Directory.CreateDirectory(xmlPath);

			exercise.initialId = searchIdArt(exercise.initialArt);
			exercise.finalId = searchIdArt(exercise.finalArt);

			exercise.rotIni.x = Mathf.Round(rotIni.x).ToString();
			exercise.rotIni.y = Mathf.Round(rotIni.y).ToString();
			exercise.rotIni.z = Mathf.Round(rotIni.z).ToString();

			exercise.rotEnd.x = Mathf.Round(rotFin.x).ToString();
			exercise.rotEnd.y = Mathf.Round(rotFin.y).ToString();
			exercise.rotEnd.z = Mathf.Round(rotFin.z).ToString();


			for (var i = 0; i < exercise.Restrictions.Count; i++) {
				exercise.Restrictions[i].initialId = searchIdArt(exercise.Restrictions[i].initialArt);
				exercise.Restrictions[i].finalId = searchIdArt(exercise.Restrictions[i].finalArt);
			}

			exercise.Save(Path.Combine(xmlPath, file + ".xml"));

			StartCoroutine(ShowMessage(5));
		}
		if (condMessage) {
			GUI.Label(new Rect(10, 10, 200, 30), "File saved!");
		}
	}
	
	public void ShowMovement() {
		if (GameObject.Find(exercise.finalArt).GetComponent<TrailRenderer>() == null) {
			GameObject.Find(exercise.initialArt).transform.eulerAngles = rotIni;

			TrailRenderer trail = GameObject.Find(exercise.finalArt).AddComponent<TrailRenderer>();
			
//			trail.material = new Material (Shader.Find("Particles/Additive"));

			trail.startWidth = 0.1f;
			trail.endWidth = 0.01f;
			trail.time = Mathf.Infinity;

			StartCoroutine (RotateMe(trail));
		}

	}

	IEnumerator RotateMe(TrailRenderer t) {
		Vector3 actualRot = GameObject.Find(exercise.initialArt).transform.eulerAngles;
		while ((Mathf.Round(actualRot.x) != Mathf.Round(rotFin.x)) ||
		       (Mathf.Round(actualRot.z) != Mathf.Round(rotFin.z))) {
			GameObject.Find(exercise.initialArt).transform.Rotate(Vector3.left * 1f);
			actualRot = GameObject.Find(exercise.initialArt).transform.eulerAngles;
			yield return null;
		}
		GameObject.Find(exercise.initialArt).transform.eulerAngles = rotFin;
		Destroy(t);
	}

	IEnumerator ShowMessage(float delay) {
		condMessage = true;
		yield return new WaitForSeconds(delay);
		condMessage = false;
		ResetExercise();
	}
	
	
	
	/****************************************************************
     * Funcion searchIdArt:
     * A esta funcion se le pasa el nombre de una articulacion
     * y te devuelve el ID correspondiente.
     ****************************************************************/
	public int searchIdArt (string name) {
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
	
	
	
	/****************************************************************
  	 * Funcion artSelection:
     * Mediante esta funcion seleccionaremos las articulaciones
     * que intervendran en el movimiento
     ****************************************************************/
	public void artSelection() {
		if (Input.GetMouseButtonUp(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			string nameInitialArt = exercise.initialArt;
			string nameFinalArt = exercise.finalArt;
			Color iniColor = Color.green;
			Color finalColor = Color.blue;
			
			if (condRest) {
				iniColor = Color.black;
				finalColor = Color.black;
				nameInitialArt = restriction.initialArt;
				nameFinalArt = restriction.finalArt;
			}
			else {
				iniColor = Color.green;
				finalColor = Color.blue;
				nameInitialArt = exercise.initialArt;
				nameFinalArt = exercise.finalArt;
			}
			
			if ((Physics.Raycast(ray, out hit)) && ((Time.time-lastClick) > catchTime)) {
				
				if ((!selectInitial) && (!selectFinal)) {
					nameInitialArt = hit.collider.gameObject.name;
					hit.collider.gameObject.renderer.material.color = iniColor;
					selectInitial = true;
				}
				else if ((selectInitial) && (!selectFinal) && (hit.collider.gameObject.name.Equals(nameInitialArt))) {
					nameInitialArt = "";
					hit.collider.gameObject.renderer.material = wood;
					selectInitial = false;
					move = false;
				}
				else if ((selectInitial) && (!selectFinal) && (!hit.collider.gameObject.name.Equals(nameInitialArt))) {
					nameFinalArt = hit.collider.gameObject.name;
					hit.collider.gameObject.renderer.material.color = finalColor;
					
					selectFinal = true;
					move = true;  //variable para poder mover solo la articulacion cuando selecciones la art final
				}
				else if ((selectInitial) && (selectFinal) && (hit.collider.gameObject.name.Equals(nameFinalArt))) {
					nameFinalArt = "";
					hit.collider.gameObject.renderer.material = wood;
					selectFinal = false;
					move = false;
				}
			}
			
			if (condRest){
				restriction.initialArt = nameInitialArt;
				restriction.finalArt = nameFinalArt;
			}
			else {
				exercise.initialArt = nameInitialArt;
				exercise.finalArt = nameFinalArt;
			}
			
			lastClick = Time.time;
		}
	}
	

	public void moveArt() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		string nameInitialArt;
		
		if (condRest)
			nameInitialArt = restriction.initialArt;
		else
			nameInitialArt = exercise.initialArt;
		
		if (Input.GetMouseButton(0)) {		
			if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals(nameInitialArt))) {
				hit.collider.gameObject.transform.Rotate(Vector3.left * 0.5f);
				varMax += 0.5f;
			}
		}
		
		if ((varMax % 360) == 0)
			varMax = 0;
	}
}
