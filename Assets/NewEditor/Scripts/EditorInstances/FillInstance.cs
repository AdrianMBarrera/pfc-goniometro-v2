using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FillInstance : MonoBehaviour {

	private Text nameExercise;

	// Use this for initialization
	void Start () {
		nameExercise = GameObject.Find("ExerciseText").GetComponent<Text>();
	}


	// Update is called once per frame
	void Update () {
	

	}
	
	public void FillFields(){

		nameExercise.text = "Exercise: " + transform.name;

	}


}
