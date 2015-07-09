using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class SaveExercisesScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void Save (){

		ExerciseValue e = new ExerciseValue();
		XmlDocument xDoc = new XmlDocument();
		switch (InfoPlayer.gameMode) {
			
		case(InfoPlayer.gameModes.Open):
			string nameParent = transform.parent.name; //coger el valor del nombre del objeto padre que es el nombre del fichero xml
			e.FileName = nameParent;
			AddExerciseList(e);
			break;
			
		case (InfoPlayer.gameModes.Custom) :

			/* TODO Distintas instancias con el mismo ejercicio, ¿se podrian ejecutar? */

			xDoc.Load("./Instances/" + transform.parent.name);
			XmlNodeList instance = xDoc.GetElementsByTagName("INSTANCE");
			e.FileName = instance[0].Attributes["name"].InnerText;
			e.InstanceRepetitions = System.Convert.ToInt16(instance[0].Attributes["repetitions"].InnerText);
			e.InstanceTime = System.Convert.ToInt16(instance[0].Attributes["time"].InnerText);
			AddExerciseList(e);
			break;
			
			
		case (InfoPlayer.gameModes.Preset) : 
			InfoPlayer.alExercise.Clear();
			xDoc.Load("./Plans/" + name);
			XmlNodeList nodeInstance = xDoc.GetElementsByTagName("nameInstance");
			
			for (int i = 0; i < nodeInstance.Count; i++) {
				string nameInstance = nodeInstance[i].InnerText;
				xDoc.Load("./Instances/" + nameInstance);
				XmlNodeList inst = xDoc.GetElementsByTagName("INSTANCE");
				e.FileName = inst[0].Attributes["name"].InnerText;
				e.InstanceRepetitions = System.Convert.ToInt16(inst[0].Attributes["repetitions"].InnerText);
				e.InstanceTime = System.Convert.ToInt16(inst[0].Attributes["time"].InnerText);
				AddExerciseList(e);
			}
			break;
		}
	}

	void AddExerciseList(ExerciseValue e) {
		bool found = false;
		int i = 0;
		while ((i < InfoPlayer.alExercise.Count) && (!found)){
			if (InfoPlayer.alExercise[i].FileName == e.FileName){
				found = true;
				InfoPlayer.alExercise.RemoveAt(i);
			}
			i++;
		}
		if (!found)
			InfoPlayer.alExercise.Add(e);
	}





}
