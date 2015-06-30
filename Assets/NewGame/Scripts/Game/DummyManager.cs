using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DummyManager : MonoBehaviour {

	public static DummyManager instance;

	public GameObject IniSphere;
	//public GameObject EndSphere;
	public GameObject ReferenceSphere;
	public GameObject RestrictSphere;
	
	private int artIni, artEnd = 0;
	private float _minimo, _maximo;
	private Vector3 _plane; // plano de medicion->definido en el fichero de definiciones
	private Vector3 _initBone; //posicion inicial del brazo, con respecto a esta posicion se medira
//	private Vector _referenceArt = new Vector(); //articulacion de referencia
	private List<Pose> _poseList = new List<Pose>();  //Lista de restricciones a tener en cuenta durante el ejercicio
	private Vector3 _rotIni; //Rotacion ArtIni
	private Vector3 _rotEnd; //Rotacion ArtFin

	private SkinnedMeshRenderer _mr;

	[Tooltip("Tamaño 15.\n0. Head\n1. Neck\n2. Spine1\n3. JointLeftArm\n4. JointLeftForeArm\n5. JointLeftHand\n" +
	         "6. JointRightArm\n7. JointRightForeArm\n8. JointRightHand\n9. LeftUpLeg\n10. JointLeftLeg\n11. LeftFoot\n" +
	         "12. RightUpLeg\n13. JointRightLeg\n14. RightFoot")]
	public GameObject[] articulations;

	public delegate void BeginExercise();
	public static event BeginExercise OnBeginExercise;

	public delegate void EndExercise();
	public static event EndExercise OnEndExercise;

	void Awake(){

		instance = this;
	}


	
	void OnEnable(){
		//GameManager.OnLoadGamePhase += ReadyAnimation;
		GameManager.OnInGamePhase += SetOff;
		GameManager.OnCalibrationPhase += SetOn;
		GameManager.OnDemostrationPhase += SetOn;
	}


	void OnDisable(){
		//GameManager.OnLoadGamePhase += ReadyAnimation;
		GameManager.OnInGamePhase -= SetOff;
		GameManager.OnCalibrationPhase -= SetOn;
		GameManager.OnDemostrationPhase -= SetOn;
	}


	void SetOff(){
		_mr.enabled = false;
	}

	void SetOn(){
		_mr.enabled = true;

	}



	// Use this for initialization
	void Start () {
	
		_mr = GetComponentInChildren<SkinnedMeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/* Cargar esfera */
	public void loadSphere(GameObject prefabSphere, int art, string name){
		//Se instancia el prefab en la posicion de la articulacion
		prefabSphere = Instantiate(prefabSphere, translateArt(art).position, Quaternion.identity) as GameObject;
		prefabSphere.name = name;
		//Se le asigna el padre, que sera la articulacion que representa, de forma que se mueva y rote con ella
		prefabSphere.transform.parent = translateArt(art);
	}
	
	
	/* Mostrar el movimiento del ejercicio */
	public void ShowMovement() {

		if ( OnBeginExercise != null){

			OnBeginExercise();
		}

		Vector3 reposePos = translateArt(artIni).eulerAngles;
		translateArt(artIni).eulerAngles = new Vector3(_rotIni.x, _rotIni.y, _rotIni.z);
		StartCoroutine (RotateArt(reposePos));
	}
	
	
	/* Corutina que hace el movimiento de la articulacion */
	IEnumerator RotateArt(Vector3 repPos) {
		
		// Restricciones
		if (_poseList.Count > 0){
			for(int i = 0; i < _poseList.Count; i++){
				_poseList[i].ReposePos = translateArt(_poseList[i].Art).eulerAngles;
				translateArt(_poseList[i].Art).transform.eulerAngles = new Vector3(_poseList[i].RotIni.x, _poseList[i].RotIni.y, _poseList[i].RotIni.z);		                              
			}
		}
		
		yield return new WaitForSeconds (1f);
		
		/** Inicio de movimiento **/
		Vector3 actualRot = translateArt(artIni).transform.eulerAngles;
		while ((Mathf.Round(actualRot.x) != Mathf.Round(_rotEnd.x)) ||
		       (Mathf.Round(actualRot.z) != Mathf.Round(_rotEnd.z))) {
			//Si el movimiento es de mas de 180º, giramos hacia el otro sentido (el mas corto)
//			if (Mathf.Round(_rotEnd.x) - translateArt(artIni).transform.rotation.x <= 180)
//				translateArt(artIni).transform.Rotate(Vector3.left * 1f);
//			else
				translateArt(artIni).transform.Rotate(Vector3.left * 1f);
			actualRot = translateArt(artIni).transform.eulerAngles;
			yield return null;
		}
		
		//		translateArt(artIni).transform.eulerAngles = rotEnd; lineal que hace que giren infinito
		
		while ((Mathf.Round(actualRot.x) != Mathf.Round(_rotIni.x)) ||
		       (Mathf.Round(actualRot.z) != Mathf.Round(_rotIni.z))) {
			//Si el movimiento es de mas de 180º, giramos hacia el otro sentido (el mas corto)
//			if (_maximo - translateArt(artIni).transform.rotation.x > 180)
//				translateArt(artIni).transform.Rotate(Vector3.left * -1f);
//			else
				translateArt(artIni).transform.Rotate(Vector3.left * -1f);
			actualRot = translateArt(artIni).transform.eulerAngles;
			yield return null;
		}
		translateArt(artIni).transform.eulerAngles = _rotIni;
		/** Fin de movimiento **/
		
		yield return new WaitForSeconds(1.5f);
		
		// Trasladamos las articulaciones de las restricciones a su posicion de reposo
		if (_poseList.Count > 0){
			for(int i = 0; i < _poseList.Count; i++)
				translateArt(_poseList[i].Art).eulerAngles = _poseList[i].ReposePos;
			_poseList.Clear();
		}
		translateArt(artIni).eulerAngles = repPos;

		deleteSphere("IniSphere");
		//	deleteSphere ("EndSphere");

		if ( OnEndExercise != null){
			OnEndExercise();
		}

		if (GameManager.instance.stateOfGame != GameManager.statesOfGame.None) {
			GameManager.instance.Calibrate();
		}
	}


	
	/* Borrar la esfera */
	public void deleteSphere(string name) {
		GameObject go = GameObject.Find(name);
		if (go != null)
			Destroy(go);
	}
	
	
	
	/* Traductor entre los joints del fichero de definicion y Carl */
	public Transform translateArt(int op) {
		switch (op) {
			case 1:	return articulations[0].transform;
			case 2: return articulations[1].transform;
			case 3: return articulations[2].transform;
			case 6: return articulations[3].transform;
			case 7: return articulations[4].transform;
			case 8: return articulations[5].transform;
			case 12: return articulations[6].transform;
			case 13: return articulations[7].transform;
			case 14: return articulations[8].transform;
			case 17: return articulations[9].transform;
			case 18: return articulations[10].transform;
			case 19: return articulations[11].transform;
			case 21: return articulations[12].transform;
			case 22: return articulations[13].transform;
			case 23: return articulations[14].transform;
			default: return articulations[0].transform;
		}
	}
	
	
	// sobrecarga del metodo 
	
	
	public void LoadXml(string name){
		
		deleteSphere("IniSphere");
		//deleteSphere ("EndSphere");
		
		XmlDocument xDoc = new XmlDocument();
		//Debug.Log (Application.dataPath);
		xDoc.Load("./Exercises/" + name);
		//Debug.Log(name);
		
		XmlNodeList exer = xDoc.GetElementsByTagName("EXERCISE");	  
		artIni = Convert.ToInt16(exer[0].Attributes["initialId"].InnerText);
		artEnd = Convert.ToInt16(exer[0].Attributes["finalId"].InnerText);
		
		XmlNodeList angles = xDoc.GetElementsByTagName("ang");
		XmlNodeList vector = xDoc.GetElementsByTagName("axis");
		XmlNodeList pos0 = xDoc.GetElementsByTagName("ini");
		XmlNodeList rotI = xDoc.GetElementsByTagName("rotIni");
		XmlNodeList rotE = xDoc.GetElementsByTagName("rotEnd");
//		XmlNodeList reference = xDoc.GetElementsByTagName ("reference");
		XmlNodeList restrictions = ((XmlElement)exer[0]).GetElementsByTagName("Restrictions");
		
		//Rotacion inicial de la articulacion principal
		_rotIni.x = Convert.ToInt16(rotI[0].Attributes["x"].InnerText);
		_rotIni.y = Convert.ToInt16(rotI[0].Attributes["y"].InnerText);
		_rotIni.z = Convert.ToInt16(rotI[0].Attributes["z"].InnerText);
		
		//Rotacion final de la articulacion principal
		_rotEnd.x = Convert.ToInt16(rotE[0].Attributes["x"].InnerText);
		_rotEnd.y = Convert.ToInt16(rotE[0].Attributes["y"].InnerText);
		_rotEnd.z = Convert.ToInt16(rotE[0].Attributes["z"].InnerText);
		
		/*	
		refId = Convert.ToInt16(reference[0].Attributes["id"].InnerText);
		referenceArt.SetX(Convert.ToInt16(reference[0].Attributes["x"].InnerText));
		referenceArt.SetY(Convert.ToInt16(reference[0].Attributes["y"].InnerText));
		referenceArt.SetZ(Convert.ToInt16(reference[0].Attributes["z"].InnerText));
	*/		
		
		XmlNodeList ID, ID1, FX, FY, FZ, RX, RY, RZ, G;
		
		//Si hay alguna restriccion en el ejercicio...
		if (restrictions.Item(0).HasChildNodes) {
			
			foreach (XmlElement restriction in restrictions) {
				int i = 0;
				Pose pose = new Pose();	
				ID = restriction.GetElementsByTagName("initialId");
				ID1 = restriction.GetElementsByTagName("finalId");
				FX = restriction.GetElementsByTagName("x");
				FY = restriction.GetElementsByTagName("y");
				FZ = restriction.GetElementsByTagName("z");
				RX = restriction.GetElementsByTagName("rotX");
				RY = restriction.GetElementsByTagName("rotY");
				RZ = restriction.GetElementsByTagName("rotZ");
				G = restriction.GetElementsByTagName("grade");
				
				//define el hueso que vamos a tener en cuenta
				pose.SetArt(Convert.ToInt16(ID[i].InnerText));
				pose.SetArt1(Convert.ToInt16(ID1[i].InnerText));
				
				//define la posicion correcta para el ejercicio
				Vector3 aux = new Vector3(Convert.ToInt16(FX[i].InnerText), 
				                          Convert.ToInt16(FY[i].InnerText), 
				                          Convert.ToInt16(FZ[i].InnerText));
				pose.SetBone(aux);
				
				pose.RotIni = new Vector3(Convert.ToInt16(RX[i].InnerText),
				                          Convert.ToInt16(RY[i].InnerText),
				                          Convert.ToInt16(RZ[i].InnerText));
				
				//define las restricciones en angulos con respecto a la posicion correcta
				pose.SetGrado(Convert.ToInt16(G[i].InnerText));
				
				//Lista de restricciones del ejercicio
				_poseList.Add (pose);
				i++; 
			}
		}
		
		//Cargamos las esferas en las articulaciones principales
		loadSphere(IniSphere, artIni, "IniSphere");
		//	loadSphere(EndSphere, artEnd, "EndSphere");

		if (GameManager.instance.stateOfGame != GameManager.statesOfGame.None) {
			GameManager.instance.artIni = artIni;
			GameManager.instance.artEnd = artEnd;

			//Angulos maximo y minimo de ejercicio
			GameManager.instance.minimo = Convert.ToInt16(angles[0].Attributes["min"].InnerText);
			GameManager.instance.maximo = Convert.ToInt16(angles[0].Attributes["max"].InnerText);

			//plano sobre el que se va a realizar la medicion
			GameManager.instance.plane = new Vector3 (Convert.ToInt16(vector[0].Attributes["x"].InnerText),
			                      Convert.ToInt16(vector[0].Attributes["y"].InnerText),
			                      Convert.ToInt16(vector[0].Attributes["z"].InnerText));
			
			//posicion de inicio del ejercicio
			GameManager.instance.initBone = new Vector3 (Convert.ToInt16(pos0[0].Attributes["x"].InnerText),
			                         Convert.ToInt16(pos0[0].Attributes["y"].InnerText),
			                         Convert.ToInt16(pos0[0].Attributes["z"].InnerText));

			GameManager.instance.poseList = new List<Pose>(_poseList);

			GameManager.instance.rsArray = new GameObject[_poseList.Count];
			int i = 0;
			foreach (Pose p in _poseList) {
				GameManager.instance.rsArray[i] = Instantiate(GameManager.instance.restrictSphere, 
				                                              translateArt(p.GetArt()).position,
				                                              Quaternion.identity) as GameObject;

				//GameManager.instance.rsArray[i].GetComponent<RestrictSphereScript>().artRest = GameObject.Find("Player/" + translateArt(p.GetArt()).name).transform;
				//GameManager.instance.rsArray[i].GetComponent<RestrictSphereScript>().artRest = translateArt(p.GetArt());
				GameManager.instance.rsArray[i].GetComponent<RestrictSphereScript>().artRest = GameObject.FindWithTag(translateArt(p.GetArt()).name).transform;
				i++;
			}
		}


		//Se ejecuta el movimiento
		ShowMovement();
	}




	//ejercicio que se va a mostrar con 







}//end class
