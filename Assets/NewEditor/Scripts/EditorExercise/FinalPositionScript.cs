using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinalPositionScript : MonoBehaviour {

	private Text min;
	private Text max;
	private Exercise exercise;
	private RotateSphere sphereScript; //Script de la esfera
	private Vector3 fin = new Vector3(0,0,0);
	private Vector3 rotFin = new Vector3(0,0,0);

	// Use this for initialization
	void Start () {
		Debug.Log ("FinalPos Start");
		exercise = GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise;
		sphereScript = GameObject.Find ("Esfera_Movimiento").GetComponent<RotateSphere>();
		min = GameObject.Find("Min").GetComponent<Text>();
		max = GameObject.Find("Max").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		sphereScript.Art = "";
		sphereScript.Step = 2;
		moveArt();
		ShowInterface();
	}


	public void moveArt() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		string nameInitialArt = exercise.initialArt;
		
		if (Input.GetMouseButton(0)) {		
			if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals(nameInitialArt))) {
				hit.collider.gameObject.transform.Rotate(Vector3.left * 0.5f);
				exercise.ang.Max = (float.Parse(exercise.ang.Max) + 0.5f).ToString();


			}
		}

		if ((float.Parse(exercise.ang.Max) % 360f) == 0)
			exercise.ang.Max = "0";
	}

	public void ShowInterface() {
		if (!exercise.finalArt.Equals("")) {
			fin = GameObject.Find(exercise.finalArt).transform.position - 
				GameObject.Find(exercise.initialArt).transform.position;

			rotFin = GameObject.Find(exercise.initialArt).transform.eulerAngles;
		}

		min.text = "Min: " + exercise.ang.Min;
		max.text = "Max: " + Mathf.Round(float.Parse(exercise.ang.Max)).ToString();
		
	}





	public void PassToManager() {

		float x = int.Parse(exercise.ini.y)*fin[2] - int.Parse(exercise.ini.z)*fin[1];
		float y = int.Parse(exercise.ini.z)*fin[0] - int.Parse(exercise.ini.x)*fin[2];
		float z = int.Parse(exercise.ini.x)*fin[1] - int.Parse(exercise.ini.y)*fin[0];
		
//		line.renderer.enabled = true;
//		line.SetPosition(0, GameObject.Find(exercise.initialArt).transform.position);
//		line.SetPosition(1, GameObject.Find(exercise.initialArt).transform.position-exercise.axis);
		
		exercise.axis.X = Mathf.Round(x).ToString();
		exercise.axis.Y = Mathf.Round(y).ToString();
		exercise.axis.Z = Mathf.Round(z).ToString();

		exercise.ang.Max = Mathf.Round(float.Parse(exercise.ang.Max)).ToString();

		exercise.rotEnd.x = Mathf.Round(rotFin.x).ToString();
		exercise.rotEnd.y = Mathf.Round(rotFin.y).ToString();
		exercise.rotEnd.z = Mathf.Round(rotFin.z).ToString();

		GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise = exercise;
	}


	public void ResetFinal() {
		exercise = GameObject.Find("ManagerInterface").GetComponent<ManagerExerciseEditor>().exercise;
		min = GameObject.Find("Min").GetComponent<Text>();
		max = GameObject.Find("Max").GetComponent<Text>();
		min.text = "Min: ";
		max.text = "Max: ";
	}
}


