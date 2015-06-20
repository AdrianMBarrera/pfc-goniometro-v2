using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class SaveScript : MonoBehaviour {

	private Text nameFile;
	private Exercise exercise; //Ejercicio que estamos editando sacado del manager exercise
	private Material wood;
	private Button saveButton;
	private Button showButton;
	private Text helpText;
	public Material trailMaterial;
	public Transform label;
	private RotateSphere sphereScript; //Script de la esfera

	void Start(){
		wood = GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().wood;
		nameFile = GameObject.Find("FileInputField").GetComponentInChildren<Text>();
		exercise = GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise;
		showButton = GameObject.Find("ShowButton").GetComponent<Button>();
		saveButton = GameObject.Find("SaveExerciseButton").GetComponent<Button>();
		helpText = GameObject.Find("HelpText").GetComponent<Text>();
		sphereScript = GameObject.Find ("Esfera_Movimiento").GetComponent<RotateSphere>();
	}

	void Update() {
		if (exercise.finalArt.Equals ("")) {
			saveButton.interactable = false;
			showButton.interactable = false;
		}
		else {
			saveButton.interactable = true;
			showButton.interactable = true;
		}
	}

	public void SaveExercise(){
		if ((nameFile.text != null) && (nameFile.text.CompareTo("")!= 0)){
			string xmlPath =  "./Exercises";
			if (!Directory.Exists(xmlPath))
				Directory.CreateDirectory(xmlPath);

			exercise.initialId = searchIdArt(exercise.initialArt);
			exercise.finalId = searchIdArt(exercise.finalArt);
			for (int i = 0; i < exercise.Restrictions.Count; i++) {
				exercise.Restrictions[i].initialId = searchIdArt(exercise.Restrictions[i].initialArt);
				exercise.Restrictions[i].finalId = searchIdArt(exercise.Restrictions[i].finalArt);
			}
			
			exercise.Save(Path.Combine(xmlPath, nameFile.text + ".xml"));
			
			StartCoroutine(ShowMessage(1, helpText, "File Saved!"));
	
		}

	}

	public void ShowMovement() {
		if (GameObject.Find(exercise.finalArt).GetComponent<TrailRenderer>() == null) {
		Debug.Log("exercise: " + exercise.rotIni.x);
			GameObject.Find(exercise.initialArt).transform.eulerAngles = new Vector3(int.Parse (exercise.rotIni.x),
			                                                                         int.Parse (exercise.rotIni.y),
			                                                                         int.Parse (exercise.rotIni.z));
			
			TrailRenderer trail = GameObject.Find(exercise.finalArt).AddComponent<TrailRenderer>();
			
			trail.material = trailMaterial;
			
			trail.startWidth = 0.1f;
			trail.endWidth = 0.01f;
			trail.time = Mathf.Infinity;
			
			StartCoroutine (RotateMe(trail));
		}
		
	}

	IEnumerator RotateMe(TrailRenderer t) {
		Vector3 actualRot = GameObject.Find(exercise.initialArt).transform.eulerAngles;
		while ((Mathf.Round(actualRot.x) != Mathf.Round(float.Parse (exercise.rotEnd.x))) ||
		       (Mathf.Round(actualRot.z) != Mathf.Round(float.Parse (exercise.rotEnd.z)))) {
			GameObject.Find(exercise.initialArt).transform.Rotate(Vector3.left * 1f);
			actualRot = GameObject.Find(exercise.initialArt).transform.eulerAngles;
			yield return null;
		}
		GameObject.Find(exercise.initialArt).transform.eulerAngles = new Vector3(float.Parse (exercise.rotEnd.x),
		                                                                         float.Parse (exercise.rotEnd.y),
		                                                                         float.Parse (exercise.rotEnd.z));
		Destroy(t);
	}


//	IEnumerator ShowMessage(float delay) {
//
//		yield return new WaitForSeconds(delay);
//		GUI.Label(new Rect(10, 10, 200, 30), "File saved!");
////		ResetExercise();
//	}



	IEnumerator ShowMessage(float delay, Text t, string help) {
		t.text = help;
		t.enabled = true;
		yield return new WaitForSeconds(delay);
		t.enabled = false;
	}

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



	public void ResetExercise() {

		if (!exercise.finalArt.Equals(""))
			GameObject.Find(exercise.finalArt).renderer.material = wood;
		
		if (!exercise.initialArt.Equals("")) {
			GameObject.Find(exercise.initialArt).renderer.material = wood;
			if ((exercise.initialArt.Equals("RightKnee")) || (exercise.initialArt.Equals("LeftKnee"))){
				GameObject.Find(exercise.initialArt).transform.localEulerAngles = new Vector3(0,180f,0);

			}else{
				GameObject.Find(exercise.initialArt).transform.eulerAngles = new Vector3(0,180f,0);

			}


		}

		sphereScript.Art = ""; // para poner la esfera fuera de la articulacion
		
//		if (!restriction.finalArt.Equals(""))
//			GameObject.Find(restriction.finalArt).renderer.material = wood;
//		
//		if (!restriction.initialArt.Equals("")) {
//			GameObject.Find(restriction.initialArt).renderer.material = wood;
//			GameObject.Find(restriction.initialArt).transform.eulerAngles = new Vector3(0,180,0);
//		}
		
		foreach (Restriction r in exercise.Restrictions) {
			GameObject.Find(r.initialArt).renderer.material = wood;
			GameObject.Find(r.finalArt).renderer.material = wood;
			GameObject.Find(r.initialArt).transform.eulerAngles = new Vector3(0,180f,0);
		}

		GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise = new Exercise();
//		GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise.Restrictions = new List<Restriction>();
//
//		exercise = new Exercise();
////		restriction = new Restriction();

		//RestrictionScript rs = GameObject.Find("RestrictionsInterface").GetComponent<RestrictionScript>();
		Destroy(GameObject.Find("RestrictionsInterface").GetComponent<RestrictionScript>());
		//GameObject.Find("RestrictionsInterface").AddComponent<RestrictionScript>();
		GameObject.Find("RestrictionsInterface").AddComponent<RestrictionScript>().labelRestriction = label;

		Destroy(GameObject.Find("StartPositionInterface").GetComponent<StartPositionScript>());
		GameObject.Find("StartPositionInterface").AddComponent<StartPositionScript>();

		Destroy(GameObject.Find("FinalPositionInterface").GetComponent<FinalPositionScript>());
		GameObject.Find("FinalPositionInterface").AddComponent<FinalPositionScript>();

		nameFile.text = "";
//		
//		selectInitial = false;
//		selectFinal = false;
//		
////		plano.renderer.enabled = false;
////		line.renderer.enabled = false;
//		
//		nameFile.text = "";
//
////		move = false;
////		condRef = false;
////		condRest = false;
//		//		condMessage = false;

		CleanRestrictionList();

	}


	//Limpia las etiquetas que estan en la lista de restricciones 

	void CleanRestrictionList(){
		GameObject bp = GameObject.Find("ButtonPool");
		int children = bp.transform.childCount;
		for (int i = 0; i < children; ++i){
			Destroy (bp.transform.GetChild(i).gameObject);
			print("For loop: " + transform.GetChild(i).gameObject);
		}

	}


	public void StoreExerciseData() {
		if (!exercise.finalArt.Equals("")) {
			GameObject.Find ("RestrictionsInterface").SendMessage("PassToManager", SendMessageOptions.DontRequireReceiver);
			GameObject.Find ("StartPositionInterface").SendMessage("PassToManager", SendMessageOptions.DontRequireReceiver);
			GameObject.Find ("FinalPositionInterface").SendMessage("PassToManager", SendMessageOptions.DontRequireReceiver);

			exercise = GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise;
		}
	} 


}