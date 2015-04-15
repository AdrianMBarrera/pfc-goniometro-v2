#pragma strict

import System.Xml;
import System.IO;
import System.Text;

//Script de la esfera
var sphereScript : RotateSphere;

var line : LineRenderer;
//var trail : TrailRenderer;

var condRest = false;

// En esta clase almacenaremos el ejercicio
var exercise : EXERCISE;
var restriction : Restriction;
var reference : Reference;

// En esta variable iremos almacenando el fichero
var file : String = "";

var lastClick : float = 0;
var catchTime : float = 1;

//Estas variables indican si las articulaciones Inicial y Final estan seleccionadas
var selectInitial = false; // variable para controlar la seleccion de la articulacion inicial
var selectFinal = false; // variable para controlar la seleccion de la articulacion final

var ini : Vector3;
var fin : Vector3;
var eje : Vector3;

var move = false;  // no puedes mover hasta que tengas las dos articulaciones selecionadas

var toolbarInt : int = 0;
var toolbarStrings : String[] = ["Restrictions", "Start position", "Final position", "Save"];

var plano : GameObject;

var varMax : float = 0.0;


// Start
function Start () {
	plano = GameObject.CreatePrimitive(PrimitiveType.Cube);
	plano.name = "Plano";
	plano.transform.localScale.x = 0.05;
	plano.transform.localScale.y = 4;
	plano.transform.localScale.z = 3;
	plano.collider.enabled = false;
	plano.renderer.enabled = false;
	
	line = gameObject.AddComponent(LineRenderer);
	line.SetWidth(0.05, 0.05);
	line.SetVertexCount(2);
	line.material = new Material (Shader.Find("Particles/Additive"));
	line.SetColors(Color.red, Color.red);
	line.renderer.enabled = false;
}


// Update
function Update () {

}


// Interfaz
function OnGUI () {

	GUI.Box(Rect(Screen.width-Screen.width/3, 0, Screen.width/3, Screen.height),"\nMOVEMENTS INTERFACE");

	toolbarInt = GUI.Toolbar (new Rect (Screen.width - Screen.width/3 + 30, 50, 400, 25), toolbarInt, toolbarStrings);
	
	switch (toolbarInt) {
	
		case 0: condRest = true;
		        step1();
		        break;
		
		case 1: condRest = false;
		        step2();
		        break;
		
		case 2: condRest = false;
		        step3();
		        break;
		
		case 3: condRest = false;
		        saveExercise();
		        break;
		        
		default: Debug.Log("Ninguna opcion valida.");
	}

}


// Paso 1
function step1() {

	GUI.Label(Rect(Screen.width-Screen.width/4+20, 100, 200, 30), "Initial articulation: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+200, 100, 200, 30), restriction.initialArt);

	GUI.Label(Rect(Screen.width-Screen.width/4+20, 130, 200, 30), "Final articulation: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+200, 130, 200, 30), restriction.finalArt);
	
	GUI.Label(Rect(Screen.width-Screen.width/4+20 , 200, 200, 30), "Rotation: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 200, 200, 30), "X: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 230, 200, 30), "Y: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 260, 200, 30), "Z: ");
		
	GUI.Label(Rect(Screen.width-Screen.width/4+20, 330, 200, 30), "Grades: ");
	restriction.grade = parseInt(GUI.TextField(Rect(Screen.width-Screen.width/4+100, 330, 50, 20),
	                             restriction.grade.ToString(), 3));
    
	artSelection();

	if (!restriction.finalArt.Equals("")) {
		restriction.x = Mathf.Round(GameObject.Find(restriction.initialArt).transform.position.x);
		restriction.y = Mathf.Round(GameObject.Find(restriction.initialArt).transform.position.y);
		restriction.z = Mathf.Round(GameObject.Find(restriction.initialArt).transform.position.z);
		
		GUI.Label(Rect(Screen.width-Screen.width/4+150, 200, 200, 30),
				  Mathf.Round(GameObject.Find(restriction.initialArt).transform.rotation.eulerAngles.x).ToString());
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 230, 200, 30),
	    		  Mathf.Round(GameObject.Find(restriction.initialArt).transform.rotation.eulerAngles.y).ToString());
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 260, 200, 30), 
	    		  Mathf.Round(GameObject.Find(restriction.initialArt).transform.rotation.eulerAngles.z).ToString());
	}
	else {
		GUI.Label(Rect(Screen.width-Screen.width/4+150, 200, 200, 30), "0");
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 230, 200, 30), "0");
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 260, 200, 30), "0");
	}

	if (GUI.Button(Rect(Screen.width-275, Screen.height-50, 125, 30), "Add restriction")) {
	   	move = false;
	   	exercise.restrictions.Add(restriction);
	   	restriction.initialArt = "";
	   	restriction.finalArt = "";
	   	selectInitial = false;
	   	selectFinal = false;
	}
	
	sphereScript.art = restriction.initialArt;
	
//	if (GUI.Button(Rect(Screen.width-275, Screen.height-30, 125, 30), "View restrictions")) {
		for (var i = 0; i < exercise.restrictions.Count; i++) {
			GUI.Label(Rect(Screen.width-Screen.width/4+20, 360+i*30, 200, 30), "Restriction " + i + ": " + 
			exercise.restrictions[i].initialArt + ", " + exercise.restrictions[i].finalArt);
		}
//	}
}


// Paso 2
function step2() {

	GUI.Label(Rect(Screen.width-Screen.width/4+20, 100, 200, 30), "Initial articulation: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+200, 100, 200, 30), exercise.initialArt.ToString());
	
	GUI.Label(Rect(Screen.width-Screen.width/4+20, 130, 200, 30), "Final articulation: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+200, 130, 200, 30), exercise.finalArt.ToString());
	
	GUI.Label(Rect(Screen.width-Screen.width/4+20 , 160, 200, 30), "Initial position: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 160, 200, 30), "X: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 190, 200, 30), "Y: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 220, 200, 30), "Z: ");
	
	//Seleccionamos las articulaciones Inicial y Final
	artSelection();
	
	//Si ya tenemos la articulacion final seleccionada
	if (!exercise.finalArt.Equals("")) {
	
		// Muestra el vector normal (fuera de la ejecucion)
		ini = GameObject.Find(exercise.finalArt).transform.position
		    - GameObject.Find(exercise.initialArt).transform.position;
		exercise.ini.x = Mathf.Round(ini[0]).ToString();
		exercise.ini.y = Mathf.Round(ini[1]).ToString();
		exercise.ini.z = Mathf.Round(ini[2]).ToString();
		
		// Mostrar plano
		plano.transform.position = GameObject.Find(exercise.initialArt).transform.position;
		plano.transform.rotation = GameObject.Find(exercise.initialArt).transform.rotation;
		plano.renderer.material.shader = Shader.Find("Transparent/VertexLit");
//		plano.renderer.material.color = Color(255,255,255,0.7);
		plano.renderer.material.color = Color(1,1,1,0.7);
		plano.renderer.enabled = true;
		
//	    trail.enabled = true;
	    
		exercise.ang.Min = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString();
		varMax = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x);
		
		GUI.Label(Rect(Screen.width-Screen.width/4+150, 160, 200, 30),
		   Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString());
		   
		GUI.Label(Rect(Screen.width-Screen.width/4+150, 190, 200, 30),
	       Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.y).ToString());
	       
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 220, 200, 30),
	       Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.z).ToString());
	}
	else {
		exercise.ini.x = "0";
		exercise.ini.y = "0";
		exercise.ini.z = "0";
	}
	
	
	/* FALTA LA ART DE REFERENCIA */
	
	sphereScript.art = exercise.initialArt;
}


// Paso 3
function step3() {

//	trail.enabled = true;
//	trail.time = Mathf.Infinity;

	GUI.Label(Rect(Screen.width-Screen.width/4+20, 100, 200, 30), "Rotation: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+100, 100, 200, 30), "Min: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+100, 130, 200, 30), "Max: ");
	moveArt();
	
	fin = GameObject.Find(exercise.finalArt).transform.position - GameObject.Find(exercise.initialArt).transform.position;
	
	var x = ini[1]*fin[2] - ini[2]*fin[1];
	var y = ini[2]*fin[0] - ini[0]*fin[2];
	var z = ini[0]*fin[1] - ini[1]*fin[0];
	
	eje = Vector3(x,y,z);
	
	line.renderer.enabled = true;
	line.SetPosition(0, GameObject.Find(exercise.initialArt).transform.position);
	line.SetPosition(1, GameObject.Find(exercise.initialArt).transform.position-eje);
	
	exercise.eje.X = Mathf.Round(x).ToString();
	exercise.eje.Y = Mathf.Round(y).ToString();
	exercise.eje.Z = Mathf.Round(z).ToString();
	
//	Debug.DrawLine(GameObject.Find(exercise.initialArt).transform.position, GameObject.Find(exercise.initialArt).transform.position + eje, Color.magenta, 2, false);

	exercise.ang.Max = Mathf.Round(varMax).ToString();

	GUI.Label(Rect(Screen.width-Screen.width/4+160, 100, 200, 30), exercise.ang.Min);
	GUI.Label(Rect(Screen.width-Screen.width/4+160, 130, 200, 30), exercise.ang.Max);
	
	moveArt();
	
}


// Paso 4 - Guardar fichero
function saveExercise() {

	GUI.Label(Rect(Screen.width-Screen.width/4+20, 300, 200, 30), "Nombre de fichero: ");
	file = GUI.TextField(Rect(Screen.width-Screen.width/4+150, 300, 150, 20), file);
	
	if (GUI.Button(Rect(Screen.width-125, Screen.height-50, 100, 30), "Guardar")) {
		var xmlPath : String = (Application.dataPath) + "/Results";
		exercise.initialId = searchIdArt(exercise.initialArt);
		exercise.finalId = searchIdArt(exercise.finalArt);
//		exercise.reference = reference;
		for (var i = 0; i < exercise.restrictions.Count ; i++){
		   exercise.restrictions[i].initialId = searchIdArt(exercise.restrictions[i].initialArt);
		   exercise.restrictions[i].finalId = searchIdArt(exercise.restrictions[i].finalArt);
		}
		//Debug.Log("Path: " + xmlPath);
		exercise.Save(Path.Combine(xmlPath, file + ".xml"));
		
//		GUI.Label(Rect(Screen.width-Screen.width/4+20, 300, 200, 30), "Nombre de fichero: ");
	}
}



/****************************************************************
  Funcion searchIdArt:
      A esta funcion se le pasa el nombre de una articulacion
      y te devuelve el ID correspondiente.
 ****************************************************************/
function searchIdArt(name){

	var id : int;
	
	switch (name) {
		case "Cabeza"     : id = 1; break;
		case "Cuello"     : id = 2; break;
		case "Torso"      : id = 3; break;
	    case "Cintura"    : id = 4; break;
	    case "ClaviculaI" : id = 5; break;
		case "HombroI"    : id = 6; break;
		case "CodoI"      : id = 7; break;
	    case "MunecaI"    : id = 8; break;
	    case "ManoI"      : id = 9; break;
		case "DedoI"      : id = 10; break;
		case "ClaviculaD" : id = 11; break;
	    case "HombroD"    : id = 12; break;
	    case "CodoD"      : id = 13; break;
		case "MunecaD"    : id = 14; break;
		case "ManoD"      : id = 15; break;
	    case "DedoD"      : id = 16; break;
	    case "CaderaI"    : id = 17; break;
		case "RodillaI"   : id = 18; break;
		case "TobilloI"   : id = 19; break;
	    case "PieI"       : id = 20; break;
	    case "CaderaD"    : id = 21; break;
		case "RodillaD"   : id = 22; break;
		case "TobilloD"   : id = 23; break;
	    case "PieD"       : id = 24; break;
	}
	
	return id;
}



/****************************************************************
  Funcion artSelection:
      Mediante esta funcion seleccionaremos las articulaciones
      que intervendran en el movimiento
 ****************************************************************/
function artSelection() {

	if (/*(Input.GetKey(KeyCode.LeftShift)) && */(Input.GetMouseButtonUp(0))) {
		var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit : RaycastHit;
		var nameInitialArt = exercise.initialArt;
		var nameFinalArt = exercise.finalArt;
        var iniColor : Color = Color.green;
        var finalColor : Color = Color.blue;
        
        	if (condRest) {
				iniColor = Color.black;
			    finalColor = Color.black;
			    nameInitialArt = restriction.initialArt;
				nameFinalArt = restriction.finalArt;
				for (var i = 0; i < exercise.restrictions.Count; i++) {
					Debug.Log(" Antes Restriction " + i + ": " + exercise.restrictions[i].initialArt + ", " + exercise.restrictions[i].finalArt);
				}
			}
			else {
				iniColor = Color.green;
			    finalColor = Color.blue;
				nameInitialArt = exercise.initialArt;
				nameFinalArt = exercise.finalArt;
			}

		if ((Physics.Raycast(ray,hit)) && (Time.time-lastClick>catchTime)) {
		
			if ((!selectInitial) && (!selectFinal)) {
				nameInitialArt = hit.collider.gameObject.name;
				hit.collider.gameObject.renderer.material.color = iniColor;
				selectInitial = true;
			}
			else if ((selectInitial) && (!selectFinal) && (hit.collider.gameObject.name.Equals(nameInitialArt))) {
				nameInitialArt = "";
				hit.collider.gameObject.renderer.material.color = Color.white;
				selectInitial = false;
				move = false;
			}
			else if ((selectInitial) && (!selectFinal) && (!hit.collider.gameObject.name.Equals(nameInitialArt))) {
				nameFinalArt = hit.collider.gameObject.name;
				hit.collider.gameObject.renderer.material.color = finalColor;
				
				// Crear esta funcion: ActivateTrail(nameFinalArt);
//					trail = GameObject.Find(nameFinalArt).AddComponent(TrailRenderer);
//					trail.enabled = false;
//					
//					trail.material = new Material (Shader.Find("Particles/Additive"));
//					trail.material.color = Color.blue;
//					
////					var m : Material = trail.GetComponent(TrailRenderer).material;
////					m.SetColor("Color", Color.red);
//
//					trail.startWidth = 0.1f;
//					trail.endWidth = 0.1f;
//					trail.time = 0;
				//***********************************************************************
					
				selectFinal = true;
				move = true;  //variable para poder mover solo la articulacion cuando selecciones la art final
			}
			else if ((selectInitial) && (selectFinal) && (hit.collider.gameObject.name.Equals(nameFinalArt))) {
				nameFinalArt = "";
				hit.collider.gameObject.renderer.material.color = Color.white;
//				GameObject.Find(nameFinalArt).Destroy(trail);
				selectFinal = false;
				move = false;
			}
		}
		
		if (condRest){
			restriction.initialArt = nameInitialArt;
			restriction.finalArt = nameFinalArt;
			for (var j = 0; j < exercise.restrictions.Count; j++) {
				Debug.Log(" Despues Restriction " + j + ": " + exercise.restrictions[j].initialArt + ", " + exercise.restrictions[j].finalArt);
			}
		}
		else {
			exercise.initialArt = nameInitialArt;
			exercise.finalArt = nameFinalArt;
		}
		
		lastClick = Time.time;
	}

}



function moveArt() {
	var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	var hit : RaycastHit;
	var nameInitialArt;

	if (condRest)
	    nameInitialArt = restriction.initialArt;
	else
		nameInitialArt = exercise.initialArt;
	
	if (Input.GetMouseButton(0)) {		
		if ((Physics.Raycast(ray, hit)) && (hit.collider.gameObject.name.Equals(nameInitialArt))) {
			hit.collider.gameObject.transform.Rotate(Vector3.right * 0.5);
			varMax += 0.5;
		}
	}
//	else if (Input.GetMouseButton(1)) {
//		if ((Physics.Raycast(ray, hit)) && (hit.collider.gameObject.name.Equals(nameInitialArt))) {
//			hit.collider.gameObject.transform.Rotate(Vector3.right * (-0.5));
//			varMax -= 0.5;
//		}
//	}
	
	if ((varMax % 360) == 0)
		varMax = 0;

}
