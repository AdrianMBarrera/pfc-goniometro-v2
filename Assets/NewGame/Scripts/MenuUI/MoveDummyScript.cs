using UnityEngine;
using System.Collections;
using UnityEngine.UI;



public class MoveDummyScript : MonoBehaviour {

//	private GameObject ButtonPool;
//	public string buttonPool;


	void OnEnable() {
		DummyManager.OnBeginExercise += SetStateButtons;
		DummyManager.OnEndExercise += SetStateButtons;
	}
	
	void OnDisable() {
		DummyManager.OnBeginExercise -= SetStateButtons;
		DummyManager.OnEndExercise -= SetStateButtons;
	}

	void Start () {
//		ButtonPool = GameObject.Find(buttonPool);
	}

	void Update () {
	
	}

	/* Poner los botones de los movimientos interactuables o no interactuables */
	void SetStateButtons(){
		GetComponent<Button>().interactable = !GetComponent<Button>().interactable;
	}

	//Carga el ejercicio del boton llamando a la instancia DummyManager
	public void LoadExercise() {
		DummyManager.instance.LoadXml(gameObject.name);
	}

}
