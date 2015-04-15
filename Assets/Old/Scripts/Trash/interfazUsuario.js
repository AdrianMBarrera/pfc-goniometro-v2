#pragma strict

import System.Xml;
import System.IO;
import System.Text;

var plano : GameObject;

var exercise : EXERCISE;

var step : int;
var grado : String = "0";
var file : String = "";

var selectInitial = false; // variable para controlar la seleccion de la articulacion inicial
var selectFinal = false; // variable para controlar la seleccion de la articulacion final

var cond = true; // 

var move = false;  // no puedes mover hasta que tengas las dos articulaciones selecionadas

var condRef = false; // variable para saber si hay una articulacion de referencia 
var reference : Reference;
var lastClick : float = 0;
var catchTime : float = .25;
var restriction : Restriction;

var ini : Vector3;
var fin : Vector3;
var eje : Vector3;

var ejex = false;
var ejey = false;
var ejez = false;


function Start () {
	step = 1;
	plano = GameObject.CreatePrimitive(PrimitiveType.Cube);
	plano.name = "Plano";
	plano.transform.localScale.x = 0.05;
	plano.transform.localScale.y = 4;
	plano.transform.localScale.z = 3;
	plano.renderer.enabled = false;
}

function Update () {

}

function OnGUI () {

	if (step == 1)
		primerPaso();
			
	if (step == 2)
		segundoPaso();
	
	if (step == 3)
		tercerPaso();
	
	if (step < 3) {
		if (GUI.Button(Rect(Screen.width-125, Screen.height-50, 100, 30), "Siguiente")){
		   if ((step==2) && (!exercise.finalArt.Equals("")))
	   		step++;
	   	   else
	   	      if (step==1)
	   	         step++;
	   	move = false;
		}      
	}	   		
	if ((step > 1) && (step <= 3)) {
		if (GUI.Button(Rect(Screen.width-275, Screen.height-50, 100, 30), "Anterior")) {
	   		step--;
	   		cond = true; //
	   		move = false;
	   	}
	}
	
	if (step >= 3) {
		if (GUI.Button(Rect(Screen.width-125, Screen.height-50, 100, 30), "Guardar")) {
			var xmlPath : String = (Application.dataPath);
			exercise.initialId = searchIdArt(exercise.initialArt);
			exercise.finalId = searchIdArt(exercise.finalArt);
			exercise.reference = reference;
			for (var i = 0; i < exercise.restrictions.Count ; i++){
			   exercise.restrictions[i].initialId = searchIdArt(exercise.restrictions[i].initialArt);
			   exercise.restrictions[i].finalId = searchIdArt(exercise.restrictions[i].finalArt);
			   
			}
			exercise.Save(Path.Combine(xmlPath,file+".xml"));	
		}
	}
}

// funcion que contiene la interfaz del segundo paso (seleccionar restriccion)

function primerPaso() {

	GUI.Box(Rect(Screen.width-Screen.width/4, 0, Screen.width/4, Screen.height),"Edicion de ejercicio: Paso 1/3\n\nRESTRICCIONES");
	GUI.Label(Rect(Screen.width-Screen.width/4+20, 60, 200, 30), "Articulacion inicial: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+20, 90, 200, 30), "Articulacion final: ");
	
	GUI.Label(Rect(Screen.width-Screen.width/4+20, 300, 200, 30), "Grados: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+200, 60, 200, 30), restriction.initialArt);
	GUI.Label(Rect(Screen.width-Screen.width/4+200, 90, 200, 30), restriction.finalArt);
    GUI.Label(Rect(Screen.width-Screen.width/4+20 , 130, 200, 30), "Rotacion: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 130, 200, 30), "X: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 160, 200, 30), "Y: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 190, 200, 30), "Z: ");
	restriction.grade = parseInt(GUI.TextField(Rect(Screen.width-Screen.width/4+100, 300, 50, 20), restriction.grade.ToString(), 3));
	if (!restriction.finalArt.Equals("")) {
		restriction.x = Mathf.Round(GameObject.Find(restriction.initialArt).transform.position.x);
		restriction.y = Mathf.Round(GameObject.Find(restriction.initialArt).transform.position.y);
		restriction.z = Mathf.Round(GameObject.Find(restriction.initialArt).transform.position.z);
		
		GUI.Label(Rect(Screen.width-Screen.width/4+150, 130, 200, 30), Mathf.Round(GameObject.Find(restriction.initialArt).transform.rotation.eulerAngles.x).ToString());
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 160, 200, 30), Mathf.Round(GameObject.Find(restriction.initialArt).transform.rotation.eulerAngles.y).ToString());
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 190, 200, 30), Mathf.Round(GameObject.Find(restriction.initialArt).transform.rotation.eulerAngles.z).ToString());
		

	}
	else {

		GUI.Label(Rect(Screen.width-Screen.width/4+150, 130, 200, 30), "0");
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 160, 200, 30), "0");
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 190, 200, 30), "0");
		
	}
	seleccionArt();
	
    if (move)
	   moveArt();
	
	if (GUI.Button(Rect(Screen.width-275, Screen.height-50, 125, 30), "Añadir restriccion")) {
	   	move = false;
	   	exercise.restrictions.Add(restriction);
	   	selectInitial = false;
	   	selectFinal = false;
	   	
	}
}



// funcion que contiene la interfaz del segundo paso (seleccionar posicion inicial)

function segundoPaso() {

	GUI.Box(Rect(Screen.width-Screen.width/4, 0, Screen.width/4, Screen.height), "Edicion de ejercicio: Paso 2/3\n\nMOVIMIENTO: Posicion inicial");

	GUI.Label(Rect(Screen.width-Screen.width/4+20, 70, 200, 30), "Articulacion inicial: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+20, 100, 200, 30), "Articulacion final: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+200, 70, 200, 30), exercise.initialArt.ToString());
	GUI.Label(Rect(Screen.width-Screen.width/4+200, 100, 200, 30), exercise.finalArt.ToString());
	GUI.Label(Rect(Screen.width-Screen.width/4+20 , 130, 200, 30), "Posicion inicial: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 130, 200, 30), "X: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 160, 200, 30), "Y: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+120, 190, 200, 30), "Z: ");	
	seleccionArt();	
	condRef = GUI.Toggle(Rect(Screen.width-Screen.width/4+20, 220, 200, 30), condRef, " Poner articulacion referencia");

	if (!exercise.finalArt.Equals("")) {
		
		ini = GameObject.Find(exercise.finalArt).transform.position - GameObject.Find(exercise.initialArt).transform.position;
		exercise.ini.x = ini[0].ToString();
		exercise.ini.y = ini[1].ToString();
		exercise.ini.z = ini[2].ToString();
		
//		plano = GameObject.CreatePrimitive(PrimitiveType.Cube);
////		plano.name = "Plano";
//		plano.transform.localScale.x = 0.1;
//		plano.transform.localScale.y = 4;
//		plano.transform.localScale.z = 3;
		plano.transform.position = GameObject.Find(exercise.initialArt).transform.position;
		plano.transform.rotation = GameObject.Find(exercise.initialArt).transform.rotation;
		plano.renderer.material.shader = Shader.Find("Transparent/VertexLit");
		plano.renderer.material.color = Color(255,255,255,0.7);
		plano.renderer.enabled = true;
		
		//Debug.DrawLine(GameObject.Find(exercise.initialArt).transform.position, GameObject.Find(exercise.initialArt).transform.position + ini, Color.green, 2, false);
		
		GUI.Label(Rect(Screen.width-Screen.width/4+150, 130, 200, 30), Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString());
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 160, 200, 30), Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.y).ToString());
	    GUI.Label(Rect(Screen.width-Screen.width/4+150, 190, 200, 30), Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.z).ToString());
	}
	else {
		exercise.ini.x = "0";
		exercise.ini.y = "0";
		exercise.ini.z = "0";
	} 


	if (condRef) {
	    GUI.Label(Rect(Screen.width-Screen.width/4+20, 250, 200, 30), "Articulacion referencia ");
		GUI.Label(Rect(Screen.width-Screen.width/4+20 , 280, 200, 30), "Rotacion: ");
		GUI.Label(Rect(Screen.width-Screen.width/4+120, 280, 200, 30), "X: ");
		GUI.Label(Rect(Screen.width-Screen.width/4+120, 310, 200, 30), "Y: ");
		GUI.Label(Rect(Screen.width-Screen.width/4+120, 340, 200, 30), "Z: ");
		if ((!exercise.initialArt.Equals("")) && (!exercise.finalArt.Equals("")))
			if ((Input.GetKey(KeyCode.LeftShift)) && (Input.GetMouseButtonUp(0))) {
				var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				var hit : RaycastHit;
				if (Physics.Raycast(ray,hit)) {
				    GUI.Label(Rect(Screen.width-Screen.width/4+200, 250, 200, 30), hit.collider.gameObject.name);
				    reference.nameId = hit.collider.gameObject.name;
					reference.id = searchIdArt(hit.collider.gameObject.name);
					hit.collider.gameObject.renderer.material.color = Color.yellow;					
				}
				else if (hit.collider.gameObject.name.Equals(reference.nameId)) {
					reference.nameId = "";
					reference.id = 0;
					reference.x = "";
					reference.y = "";
					reference.z = "";
				}
		}
	}else if (!reference.nameId.Equals("")){
	    GameObject.Find(reference.nameId).renderer.material.color = Color.white;
		reference.nameId = "";
		reference.id = 0;
		reference.x = "";
		reference.y = "";
		reference.z = "";
	}
		
	if (move)
		moveArt();
	


}

function tercerPaso() {

	GUI.Box(Rect(Screen.width-Screen.width/4, 0, Screen.width/4, Screen.height),"Edicion de ejercicio: Paso 3/3\n\nMOVIMIENTO: Posicion final");
		
	GUI.Label(Rect(Screen.width-Screen.width/4+20, 70, 200, 30), "Eje: ");
//	ejex = GUI.Toggle(Rect(Screen.width-Screen.width/4+70, 70, 200, 30), ejex, " X");
//	ejey = GUI.Toggle(Rect(Screen.width-Screen.width/4+110, 70, 200, 30), ejey, " Y");
//	ejez = GUI.Toggle(Rect(Screen.width-Screen.width/4+150, 70, 200, 30), ejez, " Z");
	seleccionEje();
	
	GUI.Label(Rect(Screen.width-Screen.width/4+20, 100, 200, 30), "Rotacion: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+100, 100, 200, 30), "Minimo: ");
	GUI.Label(Rect(Screen.width-Screen.width/4+100, 130, 200, 30), "Maximo: ");
	moveArt();
	
	fin = GameObject.Find(exercise.finalArt).transform.position - GameObject.Find(exercise.initialArt).transform.position;
	
	var x = ini[1]*fin[2] - ini[2]*fin[1];
	var y = ini[2]*fin[0] - ini[0]*fin[2];
	var z = ini[0]*fin[1] - ini[1]*fin[0];
	
	eje = Vector3(x,y,z);
	
	exercise.eje.X = x.ToString();
	exercise.eje.Y = y.ToString();
	exercise.eje.Z = z.ToString();

	Debug.DrawLine(GameObject.Find(exercise.initialArt).transform.position, GameObject.Find(exercise.initialArt).transform.position + ini, Color.green, 2, false);
	Debug.DrawLine(GameObject.Find(exercise.initialArt).transform.position, GameObject.Find(exercise.initialArt).transform.position + fin, Color.red, 2, false);
	Debug.DrawLine(GameObject.Find(exercise.initialArt).transform.position, GameObject.Find(exercise.initialArt).transform.position + eje, Color.magenta, 2, false);
	
	if (cond) {
		cond = false;
		if (ejex)
			exercise.ang.Min = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString();
		else if (ejey)
			exercise.ang.Min = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.y).ToString();
		else if (ejez)
			exercise.ang.Min = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.z).ToString();
	}
	
	if (ejex)
		exercise.ang.Max = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.x).ToString();
	else if (ejey)
		exercise.ang.Max = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.y).ToString();
	else if (ejez)
		exercise.ang.Max = Mathf.Round(GameObject.Find(exercise.initialArt).transform.rotation.eulerAngles.z).ToString();
	
	GUI.Label(Rect(Screen.width-Screen.width/4+160, 100, 200, 30), exercise.ang.Min);
	GUI.Label(Rect(Screen.width-Screen.width/4+160, 130, 200, 30), exercise.ang.Max);

	GUI.Label(Rect(Screen.width-Screen.width/4+20, 300, 200, 30), "Nombre de fichero: ");
	file = GUI.TextField(Rect(Screen.width-Screen.width/4+150, 300, 150, 20), file);
	
}


// Funcion para hallar el vector INI
//function calculateIni(iniPos, finPos) {
//	var vect : Vector3;
//	vect = finPos.transform.position - iniPos.transform.position;
//	return vect;	
//}



// funcion para buscar el id de una articulacion

function searchIdArt(name){
var id : int ;
	switch(name)
	{

		case "Cabeza": id = 1; break;
		case "Cuello" : id= 2; break;
		case "Torso" : id = 3; break;
	    case "Cintura" : id = 4; break;
	    case "ClaviculaI": id = 5; break;
		case "HombroI" : id = 6; break;
		case "CodoI" : id = 7; break;
	    case "MunecaI" : id = 8; break;
	    case "ManoI": id = 9; break;
		case "DedoI" : id = 10; break;
		case "ClaviculaD" : id = 11; break;
	    case "HombroD" : id = 12; break;
	    case "CodoD": id = 13; break;
		case "MunecaD" : id = 14; break;
		case "ManoD" : id = 15; break;
	    case "DedoD" : id = 16; break;
	    case "CaderaI": id = 17; break;
		case "RodillaI" : id = 18; break;
		case "TobilloI" : id = 19; break;
	    case "PieI" : id = 20; break;
	    case "CaderaD": id = 21; break;
		case "RodillaD" : id = 22; break;
		case "TobilloD" : id = 23; break;
	    case "PieD" : id = 24; break;
	}
	return id;
}


function seleccionArt() {
	if ((Input.GetKey(KeyCode.LeftShift)) && (Input.GetMouseButtonUp(0))) {
		var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit : RaycastHit;
		var nameInitialArt = exercise.initialArt;
		var nameFinalArt = exercise.finalArt;
        var iniColor : Color;
        var finalColor : Color;

		if (step ==1)
		{
			iniColor = Color.black;
		    finalColor = Color.black;
		    nameInitialArt = restriction.initialArt;
			nameFinalArt = restriction.finalArt;
		}
		else
		{
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
				selectFinal = true;
				move = true;  //variable para poder mover solo la articulacion cuando selecciones la art final
			}
			else if ((selectInitial) && (selectFinal) && (hit.collider.gameObject.name.Equals(nameFinalArt))) {
				nameFinalArt = "";
				hit.collider.gameObject.renderer.material.color = Color.white;
				selectFinal = false;
				move = false;
			}
		}
		
		
		if (step == 1){
			restriction.initialArt = nameInitialArt;
			restriction.finalArt = nameFinalArt;
		}
		else
		{
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

	if (step ==1)
	    nameInitialArt = restriction.initialArt;
	else
		nameInitialArt = exercise.initialArt;
	
	if (Input.GetMouseButton(0)) {		
		if ((Physics.Raycast(ray, hit)) && (hit.collider.gameObject.name.Equals(nameInitialArt))) {
		//	hit.collider.gameObject.renderer.material.color = Color.red;
			hit.collider.gameObject.transform.rotation = rotarArt(hit.collider.gameObject, 0);
		}
	}
	else if (Input.GetMouseButton(1)) {
		if ((Physics.Raycast(ray, hit)) && (hit.collider.gameObject.name.Equals(nameInitialArt))) {
		//	hit.collider.gameObject.renderer.material.color = Color.red;
			hit.collider.gameObject.transform.rotation = rotarArt(hit.collider.gameObject, 1);
		}
	}

}


function rotarArt(articulacion : GameObject, tecla) {
	var newRotation : Quaternion;
	var rot : GameObject = articulacion;

	newRotation = Quaternion.Euler(0.0,0.0,0.0);
	
	if (tecla == 0) {
		if (Camera.main.name.Equals("CamaraFrontal"))
			newRotation = Quaternion.Euler(0.0,0.0,0.5)*rot.transform.rotation;
		else if (Camera.main.name.Equals("CamaraDerecha"))
			newRotation = Quaternion.Euler(0.5,0.0,0.0)*rot.transform.rotation;
		else if (Camera.main.name.Equals("CamaraIzquierda"))
			newRotation = Quaternion.Euler(-0.5,0.0,0.0)*rot.transform.rotation;
		else if (Camera.main.name.Equals("CamaraCenital"))
			newRotation = Quaternion.Euler(0.0,0.5,0.0)*rot.transform.rotation;
	}
	else if (tecla == 1) {
		if (Camera.main.name.Equals("CamaraFrontal"))
			newRotation = Quaternion.Euler(0.0,0.0,-0.5)*rot.transform.rotation;
		else if (Camera.main.name.Equals("CamaraDerecha"))
			newRotation = Quaternion.Euler(-0.5,0.0,0.0)*rot.transform.rotation;
		else if (Camera.main.name.Equals("CamaraIzquierda"))
			newRotation = Quaternion.Euler(0.5,0.0,0.0)*rot.transform.rotation;
		else if (Camera.main.name.Equals("CamaraCenital"))
			newRotation = Quaternion.Euler(0.0,-0.5,0.0)*rot.transform.rotation;
	}
		
	return newRotation;
}


function seleccionEje() {
	if (Camera.allCameras[0].name.Equals("CamaraFrontal")) {
		ejex = false;
		ejey = false;
		ejez = true;
	}
	else if (Camera.allCameras[0].name.Equals("CamaraCenital")) {
		ejex = false;
		ejey = true;
		ejez = false;
	}
	else if ((Camera.allCameras[0].name.Equals("CamaraDerecha")) || (Camera.allCameras[0].name.Equals("CamaraIzquierda"))) {
		ejex = true;
		ejey = false;
		ejez = false;
	}
}



