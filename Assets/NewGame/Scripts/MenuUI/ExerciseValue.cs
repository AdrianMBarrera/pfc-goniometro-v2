using UnityEngine;
using System.Collections;

public class ExerciseValue  {

	string fileName; // nombre del fichero que contiene el ejercicio
	float time;
	string resultName; //nombre del fichero resultado
	int numFail;
	int numSuccess;



	public string FileName{
		get{ return fileName;}
		set{fileName = value;}

	}



}
