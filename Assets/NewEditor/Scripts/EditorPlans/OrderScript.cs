using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class OrderScript : MonoBehaviour {

	ManagerOrderScript mo;
	Toggle toggle;

	// Use this for initialization
	void Start () {
		toggle = GetComponent<Toggle>();
		mo = GameObject.Find("ManagerOrder").GetComponent<ManagerOrderScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeMax(){
		if  (toggle.isOn){
			mo.maxExercises++;

		}
		else{
			mo.maxExercises--;

		}
	}




	
	public void ReOrder(){


		if  (toggle.isOn){
			GetComponentInChildren<Text>().text = mo.maxExercises.ToString();

		}else{
			int unCheckeValue = int.Parse(GetComponentInChildren<Text>().text);
			GetComponentInChildren<Text>().text = "-";
			foreach(GameObject t in GameObject.FindGameObjectsWithTag("Order"))
			{
				if (t.GetComponent<Text>().text != "-"){
					int numberOrder = int.Parse(t.GetComponent<Text>().text);
					if (numberOrder > unCheckeValue){
						numberOrder = numberOrder -1;
						t.GetComponent<Text>().text = numberOrder.ToString();
					}
					
				}
			}

		}
	}
}