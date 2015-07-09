using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class SavePlanScript : MonoBehaviour {

	private Plan plan;
	private InputField nameFile;
	private Text helpText;
	ManagerOrderScript mo;
	// Use this for initialization
	void Start () {
		helpText = GameObject.Find("HelpText").GetComponent<Text>();
		nameFile = GameObject.Find("PlanInputField").GetComponent<InputField>();
		mo = GameObject.Find("UI").GetComponent<ManagerOrderScript>();
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void SavePlan(){
		if ((nameFile.text != null) && (nameFile.text.CompareTo("")!= 0)){ //faltan añadir condiciones
			plan = new Plan();
			string xmlPath =  "./Plans";
			if (!Directory.Exists(xmlPath)){
				Directory.CreateDirectory(xmlPath);
			}


			for (int i = 1; i <= mo.maxExercises; i++){
				foreach(GameObject toggle in GameObject.FindGameObjectsWithTag("Toggle"))
				{
					if (toggle.GetComponent<Toggle>().isOn){
						if (toggle.GetComponentInChildren<Text>().text == i.ToString()){
							Schedule s = new Schedule();
							s.nameInstance = toggle.transform.parent.name;
							plan.scheduleList.Add(s);
						}
					}
				}

			}

			plan.Save(Path.Combine(xmlPath, nameFile.text + ".xml"));
			StartCoroutine(ShowMessage(4, helpText, "File Saved!"));
		}
		else{

			StartCoroutine(ShowMessage(4, helpText, "Must select an instance!"));

		}


	}



	IEnumerator ShowMessage(float delay, Text t, string help) {
		t.text = help;
		t.enabled = true;
		yield return new WaitForSeconds(delay);
		t.enabled = false;
	}

}
