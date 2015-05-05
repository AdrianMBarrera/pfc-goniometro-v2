using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//Script que controla las camaras que se esta utilizando*/

public class CameraSelection : MonoBehaviour {

	public Camera frontCam; // camara frontal
	public Camera backCam; // camara frontal anterior
	public Camera rightCam; // camara tranversal derecha
	public Camera leftCam; // camara tranversal izquierda
	public Camera aerialCam; // camara sagital
	Button frontButton;
	Button backButton;
	Button rightButton;
	Button leftButton;	
	Button aerialButton;
	private ColorBlock cbPressed;
	private ColorBlock cbNormal;


	// Use this for initialization
	void Start () {
		frontCam.enabled = true;
		backCam.enabled	= false;
		rightCam.enabled = false;
		leftCam.enabled = false;
		aerialCam.enabled = false;
		frontButton = GameObject.Find("FrontalButton").GetComponent<Button>();
		backButton = GameObject.Find("AnteriorButton").GetComponent<Button>();
		rightButton = GameObject.Find("TransversalRButton").GetComponent<Button>();
		leftButton = GameObject.Find("TransversalLButton").GetComponent<Button>();	
		aerialButton = GameObject.Find("SagittalButton").GetComponent<Button>();

		frontButton.onClick.AddListener( delegate {ChangeColor(frontButton, frontCam);});
		backButton.onClick.AddListener(delegate {ChangeColor(backButton, backCam);});
		rightButton.onClick.AddListener(delegate {ChangeColor(rightButton, rightCam);});
		leftButton.onClick.AddListener(delegate {ChangeColor(leftButton, leftCam);});
		aerialButton.onClick.AddListener(delegate {ChangeColor(aerialButton, aerialCam);});


		cbNormal = frontButton.colors;
		cbPressed = frontButton.colors;
		cbPressed.normalColor = Color.gray;

	}


	public void ChangeColor(Button b, Camera go){
		frontButton.colors = cbNormal;
		backButton.colors = cbNormal;
		rightButton.colors = cbNormal;
		leftButton.colors = cbNormal;	
		aerialButton.colors = cbNormal;
		b.colors = cbPressed;
		frontCam.enabled = false;
		backCam.enabled	= false;
		rightCam.enabled = false;
		leftCam.enabled = false;
		aerialCam.enabled = false;
		go.enabled = true;

	}


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("f1")) {
			frontCam.enabled = true;
			backCam.enabled	= false;
			rightCam.enabled = false;
			leftCam.enabled = false;
			aerialCam.enabled = false;
			frontButton.colors = cbPressed;
			backButton.colors = cbNormal;
			rightButton.colors = cbNormal;
			leftButton.colors = cbNormal;	
			aerialButton.colors = cbNormal;

		}
		
		if (Input.GetKeyDown("f2")) {
			frontCam.enabled = false;
			backCam.enabled	= true;
			rightCam.enabled = false;
			leftCam.enabled = false;
			aerialCam.enabled = false;
			frontButton.colors = cbNormal;
			backButton.colors = cbPressed;
			rightButton.colors = cbNormal;
			leftButton.colors = cbNormal;	
			aerialButton.colors = cbNormal;
		}
		
		if (Input.GetKeyDown("f5")) {
			frontCam.enabled = false;
			backCam.enabled	= false;
			rightCam.enabled = true;
			leftCam.enabled = false;
			aerialCam.enabled = false;
			frontButton.colors = cbNormal;
			backButton.colors = cbNormal;
			rightButton.colors = cbPressed;
			leftButton.colors = cbNormal;	
			aerialButton.colors = cbNormal;
		}
		
		if (Input.GetKeyDown("f4")) {
			frontCam.enabled = false;
			backCam.enabled	= false;
			rightCam.enabled = false;
			leftCam.enabled = true;
			aerialCam.enabled = false;
			frontButton.colors = cbNormal;
			backButton.colors = cbNormal;
			rightButton.colors = cbNormal;
			leftButton.colors = cbPressed;	
			aerialButton.colors = cbNormal;
		}
		
		if (Input.GetKeyDown("f3")) {
			frontCam.enabled = false;
			backCam.enabled	= false;
			rightCam.enabled = false;
			leftCam.enabled = false;
			aerialCam.enabled = true;
			frontButton.colors = cbNormal;
			backButton.colors = cbNormal;
			rightButton.colors = cbNormal;
			leftButton.colors = cbNormal;	
			aerialButton.colors = cbPressed;
		}
	}
}
