using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveExercisesScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void Save (){
		string nameParent = transform.parent.name; //coger el valor del nombre del objeto padre que es el nombre del fichero xml

		ExerciseValue e = new ExerciseValue();
		e.FileName = nameParent;
		bool found = false;
		int i = 0;

		while ((i < InfoPlayer.alExercise.Count) && (!found)){

			if (InfoPlayer.alExercise[i].FileName == e.FileName){
				found = true;

				InfoPlayer.alExercise.RemoveAt(i);
			}
			i++;
		}

		if (!found){

			InfoPlayer.alExercise.Add(e);
		}





	}





}
