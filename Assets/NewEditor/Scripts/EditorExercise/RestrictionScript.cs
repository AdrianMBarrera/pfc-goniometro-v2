using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RestrictionScript : MonoBehaviour {

	private Exercise exercise;
	private Text initArt;
	private Text finalArt;
	private Text restX;
	private Text restY;
	private Text restZ;
	private Text grades;
	private Restriction restriction;

	private bool selectInitial = false; // variable para controlar la seleccion de la articulacion inicial de restriccion
	private bool selectFinal = false; // variable para controlar la seleccion de la articulacion final de restriccion
	private float lastClick = 0.0f;
	private float catchTime = 1.0f;
	private RotateSphere sphereScript; //Script de la esfera
	private Material wood;
	private Button addRestricctionButton;
//	private int cont = 0;

	public Transform labelRestriction;

	// Use this for initialization
	void Start () {
		addRestricctionButton = GameObject.Find("AddRestriction").GetComponent<Button>();
		addRestricctionButton.onClick.RemoveAllListeners();
		addRestricctionButton.onClick.AddListener(() => {AddRestriction();});
		//addRestricctionButton.onClick.AddListener(() => {AddRestrictionLabel();});
		wood = GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().wood;
		sphereScript = GameObject.Find ("Esfera_Movimiento").GetComponent<RotateSphere>();
		exercise = GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise;
		restriction = new Restriction();
		initArt = GameObject.Find("RestInitialArticulation").GetComponent<Text>();
		finalArt = GameObject.Find("RestFinalArticulation").GetComponent<Text>();
		restX = GameObject.Find("RestX").GetComponent<Text>();
		restY = GameObject.Find("RestY").GetComponent<Text>();
		restZ = GameObject.Find("RestZ").GetComponent<Text>();
		grades = GameObject.Find("GradesInputField").GetComponentInChildren<Text>();
	}

	// Update is called once per frame
	void Update () {
		sphereScript.Step = 0;
		if (Input.GetMouseButtonUp(0)){
			artSelection();
		}
		ShowInterface();
	}


	void ShowInterface(){

		initArt.text = "Initial articulation: " + restriction.initialArt;
		finalArt.text = "Final articulation: " + restriction.finalArt;
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

			restX.text = "X: " + restriction.rotX.ToString();
			restY.text = "Y: " + restriction.rotY.ToString();
			restZ.text = "Z: " + restriction.rotZ.ToString();
		}
		else {
			restX.text = "X: ";
			restY.text = "Y: ";
			restZ.text = "Z: ";
		}
	}



	public void AddRestriction() {
		if ((grades.text != null) && (grades.text.CompareTo("")!= 0))
			restriction.grade =  int.Parse(grades.text);
		//Debug.Log ("Restriction " + restriction.initialArt);
		exercise.Restrictions.Add(restriction);
		AddRestrictionLabel();
		restriction = new Restriction();
		selectInitial = false;
		selectFinal = false;
		sphereScript.Art = "";
	}


	public void AddRestrictionLabel(){
		//Transform label = (Instantiate(labelRestriction, labelRestriction.transform.position, labelRestriction.transform.rotation) as Transform);
		Transform label = (Instantiate(labelRestriction) as Transform);
		label.SetParent(GameObject.Find("ButtonPool").transform);
		label.transform.localScale = new Vector3 (1,1,1);
		label.name = restriction.initialArt + "," + restriction.finalArt;

		label.GetComponentInChildren<Text>().text = label.name;
	}


	public void DeleteRestriction (string name) {
		string[] nameR = name.Split(',');
		//Debug.Log (nameR.Length);
		//Debug.Log (nameR[0] + "," + nameR[1]);
		//foreach (Restriction r in exercise.Restrictions) {
		for (int i = 0; i < exercise.Restrictions.Count; i++) {
			if (exercise.Restrictions[i].initialArt.Equals(nameR[0]) && exercise.Restrictions[i].finalArt.Equals(nameR[1])) {
				//Debug.Log ("Ini:" + r.initialArt + " Fin:" + r.finalArt);
				GameObject.Find(exercise.Restrictions[i].initialArt).renderer.material = wood;
				GameObject.Find(exercise.Restrictions[i].finalArt).renderer.material = wood;
				GameObject.Find(exercise.Restrictions[i].initialArt).transform.eulerAngles = new Vector3(0,180,0);
				exercise.Restrictions.RemoveAt(i);
			}
		}
	}



	public void PassToManager(){
		foreach (Restriction r in exercise.Restrictions) {
			Debug.Log ("Restriction: " + r.initialArt);
		}

		GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise = exercise;
	}




	public void artSelection() {
		 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			Color iniColor = Color.black;
			Color finalColor = Color.black;
			
			if ((Physics.Raycast(ray, out hit)) && ((Time.time-lastClick) > catchTime)) {
				
				if ((!selectInitial) && (!selectFinal)) {

					restriction.initialArt = hit.collider.gameObject.name;
					hit.collider.gameObject.renderer.material.color = iniColor;
					selectInitial = true;

				}
				else if ((selectInitial) && (!selectFinal) &&
			         (hit.collider.gameObject.name.Equals(restriction.initialArt))) {

					restriction.initialArt = "";
					hit.collider.gameObject.renderer.material = wood;
					sphereScript.Art = "";
					selectInitial = false;
				}
				else if ((selectInitial) && (!selectFinal) &&
			         (!hit.collider.gameObject.name.Equals(restriction.initialArt))) {

					hit.collider.gameObject.renderer.material.color = finalColor;
					selectFinal = true;
					restriction.finalArt = hit.collider.gameObject.name; // poner en la gui art inicial
					sphereScript.Art = restriction.initialArt; // Articulacion que tiene que rotar la esfera 
				}
				else if ((selectInitial) && (selectFinal) &&
			         (hit.collider.gameObject.name.Equals(restriction.finalArt))) {

					restriction.finalArt = "";
					hit.collider.gameObject.renderer.material = wood;
					selectFinal = false;
					sphereScript.Art = "";
				}
			}

			lastClick = Time.time;

	}

}
