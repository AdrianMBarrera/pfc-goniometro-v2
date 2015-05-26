using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfoPlayer : MonoBehaviour {

	public enum gameModes: int {Open = 1, Custom = 2, Preset = 3}


	public static int gameMode = -1;

	public static List<ExerciseValue> alExercise = new List <ExerciseValue>();

}
