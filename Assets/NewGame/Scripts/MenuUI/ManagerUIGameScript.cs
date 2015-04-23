using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.IO;
using System.Text;


public class ManagerUIGameScript : MonoBehaviour {

	private ColorBlock cbPressed;
	private ColorBlock cbNormal;
	
	private Canvas exerciseI;
	private Canvas instanceI;
	private Canvas planI;
	
	private Button exerciseTab;
	private Button instanceTab;
	private Button planTab;

	private Text helper; // texto de ayuda
	private LoadingScript loading; // llamar a pasar de nivel
	
	// Use this for initialization
	void Start () {
		exerciseI = GameObject.Find("InterfaceExercise").GetComponent<Canvas>();
		instanceI = GameObject.Find("InterfaceInstance").GetComponent<Canvas>();
		planI = GameObject.Find("InterfacePlan").GetComponent<Canvas>();

		exerciseTab = GameObject.Find("TabExercise").GetComponent<Button>();
		instanceTab = GameObject.Find("TabInstance").GetComponent<Button>();
		planTab	= GameObject.Find("TabPlan").GetComponent<Button>();

		cbNormal = exerciseTab.colors;
		cbPressed = exerciseTab.colors;
		cbPressed.normalColor = exerciseTab.colors.pressedColor;

		helper = GameObject.Find("TextHelp").GetComponent<Text>();
		helper.enabled = false;
		helper.text = "Please, Select any exercise";
		loading = GameObject.Find("Veil").GetComponent <LoadingScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (exerciseI.enabled) {
			/*exerciseI.GetComponent<GameExerciseScript>().enabled = true;
			instanceI.GetComponent<GameInstanceScript>().enabled = false;
			planI.GetComponent<GamePlanScript>().enabled = false;*/
			
			exerciseTab.colors = cbPressed;
			instanceTab.colors = cbNormal;
			planTab.colors= cbNormal;
		}
		
		if (instanceI.enabled) {
			/*exerciseI.GetComponent<GameExerciseScript>().enabled = false;
			instanceI.GetComponent<GameInstanceScript>().enabled = true;
			planI.GetComponent<GamePlanScript>().enabled = false;*/
			
			exerciseTab.colors = cbNormal;
			instanceTab.colors = cbPressed;
			planTab.colors= cbNormal;
		}
		
		if (planI.enabled) {
			/*exerciseI.GetComponent<GameExerciseScript>().enabled = false;
			instanceI.GetComponent<GameInstanceScript>().enabled = false;
			planI.GetComponent<GamePlanScript>().enabled = true;*/
			
			exerciseTab.colors = cbNormal;
			instanceTab.colors = cbNormal;
			planTab.colors= cbPressed;
		}
	}

	public void PlayGame (string nameLevel) {
		if (exerciseI.enabled) {
			if (InfoPlayer.alExercise.Count < 1)
				StartCoroutine("HelpCoroutine");
			else
				loading.BeginLevel(nameLevel);
		}
		
		/*else if (instanceI.enabled) {

		}
		
		else if (planI.enabled) {

		}*/

	}

	IEnumerator HelpCoroutine() {
		helper.enabled = true;
		yield return new WaitForSeconds(3f);
		helper.enabled = false;
	}
	
}

