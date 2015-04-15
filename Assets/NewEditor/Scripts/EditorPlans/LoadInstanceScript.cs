using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;


public class LoadInstanceScript : MonoBehaviour {

	private MoveDummyScript moveDummyScript;
	private Text nameExercise;
	private Text timeExercise;
	private Text repetitionExercise;


	// Use this for initialization
	void Start () {
	
		moveDummyScript = GetComponent<MoveDummyScript>();
		nameExercise = GameObject.Find("ExerciseName").GetComponent<Text>();
		timeExercise = GameObject.Find("InstanceTime").GetComponent<Text>();
		repetitionExercise = GameObject.Find("InstanceRep").GetComponent<Text>();
	}


	// Update is called once per frame
	void Update () {
	
	}


	public void LoadInstance(){

		XmlDocument xDoc = new XmlDocument();
		Debug.Log (Application.dataPath);
		xDoc.Load("./Instances/" + name);
		Debug.Log(name);
		XmlNodeList instance = xDoc.GetElementsByTagName("INSTANCE");	
		nameExercise.text = "Exercise: " +(instance[0].Attributes["name"].InnerText);
		timeExercise.text = "Time: " + (instance[0].Attributes["time"].InnerText);
		repetitionExercise.text = "Repetitions: " + (instance[0].Attributes["repetitions"].InnerText);
		moveDummyScript.LoadXml(instance[0].Attributes["name"].InnerText);

	}





}
