using UnityEngine;
using System.Collections;

public class ShowValuesScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for (int i  = 0; i < InfoPlayer.alExercise.Count; i++){
			Debug.Log(InfoPlayer.alExercise[i].FileName);

		}



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
