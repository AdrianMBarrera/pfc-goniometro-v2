using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowInfoScript : MonoBehaviour {

	private Text nameInstance;
	private Text nameExercise;
	private Text time;
	private Text repetitions;


	// Use this for initialization
	void Start () {
		nameInstance = GameObject.Find("InstanceName").GetComponent<Text>();
		nameExercise = GameObject.Find("ExerciseName").GetComponent<Text>();
		time = GameObject.Find("InstanceTime").GetComponent<Text>();
		repetitions = GameObject.Find("InstanceRep").GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void ShowInfo(){

		nameInstance.text = "Instance: "+ transform.name;
		nameExercise.text = "Exercise: ";
		time.text ="Time: "  ;
		repetitions.text = "Repetitions: " ;


	}


}
