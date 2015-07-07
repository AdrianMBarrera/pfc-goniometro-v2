using UnityEngine;
using System.Collections;

public class ExerciseValue  {

	string fileName; // nombre del fichero que contiene el ejercicio
	float duration = 0;
	int success = 0;
	int fail = 0;
	float max = 0; //maximo en porcentaje que has llegado en un ejercicio. 
	float min = 100f; // minimo en porcentaje que has llegado en un ejercicio

	int instanceRepetitions;
	float instanceTime;

	public string FileName{
		get{ return fileName;}
		set{fileName = value;}
	}


	public float Duration{
		get{ return duration;}
		set{duration = value;}
		
	}


	public int Success{
		get{ return success;}
		set{success = value;}
	}

	public int Fail{
		get{ return fail;}
		set{fail = value;}
	}



	public float Max{
		get {return max;}
		set{max = value;}
	}

	
	public float Min{
		get {return min;}
		set{min = value;}	
	}

	public int InstanceRepetitions{
		get{ return instanceRepetitions;}
		set{instanceRepetitions = value;}
	}

	public float InstanceTime{
		get{ return instanceTime;}
		set{instanceTime = value;}
		
	}

}
