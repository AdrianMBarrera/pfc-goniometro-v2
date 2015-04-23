using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;



public class MoveDummyScript : MonoBehaviour {

	//Prefabs
	public GameObject IniSphere;
	public GameObject EndSphere;
	public GameObject ReferenceSphere;
	public GameObject RestrictSphere;
	private GameObject ButtonPool;

	private int artIni, artEnd, refId, inicio, eje = 0;
	private float minimo, maximo;
	Vector plane = new Vector(); // plano de medicion->definido en el fichero de definiciones
	Vector initBone = new Vector(); //posicion inicial del brazo, con respecto a esta posicion se medira
	Vector referenceArt = new Vector(); //articulacion de referencia
	List<Pose> poseList = new List<Pose>();  //Lista de restricciones a tener en cuenta durante el ejercicio
	private Vector3 rotIni; //Rotacion ArtIni
	private Vector3 rotEnd; //Rotacion ArtFin
	public string buttonPool;

	void Start () {
		ButtonPool = GameObject.Find(buttonPool);
	}

	void Update () {
	
	}


	/* Cargar fichero XML del ejercicio y mostrar el movimiento a realizar */
	public void LoadXml(){

		XmlDocument xDoc = new XmlDocument();
		Debug.Log (Application.dataPath);
		xDoc.Load("./Exercises/" + name);
		Debug.Log(name);
			
		XmlNodeList exer = xDoc.GetElementsByTagName("EXERCISE");	  
		artIni = Convert.ToInt16(exer[0].Attributes["initialId"].InnerText);
		artEnd = Convert.ToInt16(exer[0].Attributes["finalId"].InnerText);
			
		XmlNodeList angles = xDoc.GetElementsByTagName("ang");
		XmlNodeList vector = xDoc.GetElementsByTagName("axis");
		XmlNodeList pos0 = xDoc.GetElementsByTagName("ini");
		XmlNodeList rotI = xDoc.GetElementsByTagName("rotIni");
		XmlNodeList rotE = xDoc.GetElementsByTagName("rotEnd");
		XmlNodeList reference = xDoc.GetElementsByTagName ("reference");
		XmlNodeList restrictions = ((XmlElement)exer[0]).GetElementsByTagName("Restrictions");
		
		//Angulos maximo y minimo de ejercicio
		minimo = Convert.ToInt16(angles[0].Attributes["min"].InnerText);
		maximo = Convert.ToInt16(angles[0].Attributes["max"].InnerText);
		
		//plano sobre el que se va a realizar la medicion
		plane.SetX(Convert.ToInt16(vector[0].Attributes["x"].InnerText));
		plane.SetY(Convert.ToInt16(vector[0].Attributes["y"].InnerText));
		plane.SetZ(Convert.ToInt16(vector[0].Attributes["z"].InnerText));
		
		//posicion de inicio del ejercicio
		initBone.SetX(Convert.ToInt16(pos0[0].Attributes["x"].InnerText));
		initBone.SetY(Convert.ToInt16(pos0[0].Attributes["y"].InnerText));
		initBone.SetZ(Convert.ToInt16(pos0[0].Attributes["z"].InnerText));

		//Rotacion inicial de la articulacion principal
		rotIni.x = Convert.ToInt16(rotI[0].Attributes["x"].InnerText);
		rotIni.y = Convert.ToInt16(rotI[0].Attributes["y"].InnerText);
		rotIni.z = Convert.ToInt16(rotI[0].Attributes["z"].InnerText);

		//Rotacion final de la articulacion principal
		rotEnd.x = Convert.ToInt16(rotE[0].Attributes["x"].InnerText);
		rotEnd.y = Convert.ToInt16(rotE[0].Attributes["y"].InnerText);
		rotEnd.z = Convert.ToInt16(rotE[0].Attributes["z"].InnerText);

	/*	
		refId = Convert.ToInt16(reference[0].Attributes["id"].InnerText);
		referenceArt.SetX(Convert.ToInt16(reference[0].Attributes["x"].InnerText));
		referenceArt.SetY(Convert.ToInt16(reference[0].Attributes["y"].InnerText));
		referenceArt.SetZ(Convert.ToInt16(reference[0].Attributes["z"].InnerText));
	*/		

		XmlNodeList ID;
		XmlNodeList ID1;		
		XmlNodeList FX;
		XmlNodeList FY;
		XmlNodeList FZ;
		XmlNodeList RX;
		XmlNodeList RY;
		XmlNodeList RZ;
		XmlNodeList G;

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
				Vector aux = new Vector();
				aux.SetX(Convert.ToInt16(FX[i].InnerText));
				aux.SetY(Convert.ToInt16(FY[i].InnerText));
				aux.SetZ(Convert.ToInt16(FZ[i].InnerText));
				pose.SetBone(aux);

				pose.RotIni = new Vector3(Convert.ToInt16(RX[i].InnerText),
				                              Convert.ToInt16(RY[i].InnerText),
				                              Convert.ToInt16(RZ[i].InnerText));
				
				//define las restricciones en angulos con respecto a la posicion correcta
				pose.SetGrado(Convert.ToInt16(G[i].InnerText));
				
				//Lista de restricciones del ejercicio
				poseList.Add (pose);
				i++; 
			}
		}

		//Cargamos las esferas en las articulaciones principales
		loadSphere(IniSphere, artIni, "IniSphere");
		loadSphere(EndSphere, artEnd, "EndSphere");

		//Se ejecuta el movimiento
		ShowMovement();
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
		Vector3 reposePos = translateArt(artIni).eulerAngles;
		translateArt(artIni).eulerAngles = new Vector3(rotIni.x, rotIni.y, rotIni.z);
		StartCoroutine (RotateArt(reposePos));
	}

	
	/* Corutina que hace el movimiento de la articulacion */
	IEnumerator RotateArt(Vector3 repPos) {
		SetStateButtons(false);

		// Restricciones
		if (poseList.Count > 0){
			for(int i = 0; i < poseList.Count; i++){
				poseList[i].ReposePos = translateArt(poseList[i].Art).eulerAngles;
				translateArt(poseList[i].Art).transform.eulerAngles = new Vector3(poseList[i].RotIni.x, poseList[i].RotIni.y, poseList[i].RotIni.z);		                              
			}
		}
		
		yield return new WaitForSeconds (1f);

		/** Inicio de movimiento **/
		Vector3 actualRot = translateArt(artIni).transform.eulerAngles;
		while ((Mathf.Round(actualRot.x) != Mathf.Round(rotEnd.x)) ||
		       (Mathf.Round(actualRot.z) != Mathf.Round(rotEnd.z))) {
			translateArt(artIni).transform.Rotate(Vector3.left * 1f);
			actualRot = translateArt(artIni).transform.eulerAngles;
			yield return null;
		}

//		translateArt(artIni).transform.eulerAngles = rotEnd; lineal que hace que giren infinito

		while ((Mathf.Round(actualRot.x) != Mathf.Round(rotIni.x)) ||
		       (Mathf.Round(actualRot.z) != Mathf.Round(rotIni.z))) {
			translateArt(artIni).transform.Rotate(Vector3.left * -1f);
			actualRot = translateArt(artIni).transform.eulerAngles;
			yield return null;
		}
		translateArt(artIni).transform.eulerAngles = rotIni;
		/** Fin de movimiento **/

		yield return new WaitForSeconds(1.5f);

		// Trasladamos las articulaciones de las restricciones a su posicion de reposo
		if (poseList.Count > 0){
			for(int i = 0; i < poseList.Count; i++)
				translateArt(poseList[i].Art).eulerAngles = poseList[i].ReposePos;
			poseList.Clear();
		}
		translateArt(artIni).eulerAngles = repPos;


		deleteSphere("IniSphere");
		deleteSphere ("EndSphere");
		SetStateButtons(true);



	}





	/* Poner los botones de los movimientos interactuables o no interactuables */
	void SetStateButtons(bool state){
		foreach (Transform b in ButtonPool.transform)
			b.GetComponent<Button>().interactable = state;
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
			case 1:	return GameObject.Find ("Head").transform;
			case 2: return GameObject.Find ("Neck").transform;
			case 3: return GameObject.Find ("Spine1").transform;
			case 6: return GameObject.Find ("JointLeftArm").transform;
			case 7: return GameObject.Find ("JointLeftForeArm").transform;
			case 8: return GameObject.Find ("JointLeftHand").transform;
			case 12: return GameObject.Find ("JointRightArm").transform;
			case 13: return GameObject.Find ("JointRightForeArm").transform;
			case 14: return GameObject.Find ("JointRightHand").transform;
			case 17: return GameObject.Find ("LeftUpLeg").transform;
			case 18: return GameObject.Find ("JointLeftLeg").transform;
			case 19: return GameObject.Find ("LeftFoot").transform;
			case 21: return GameObject.Find ("RightUpLeg").transform;
			case 22: return GameObject.Find ("JointRightLeg").transform;
			case 23: return GameObject.Find ("RightFoot").transform;
			default: return GameObject.Find ("Head").transform;
		}
	}


	// sobrecarga del metodo 


	public void LoadXml(string name){
		
		deleteSphere("IniSphere");
		deleteSphere ("EndSphere");
		
		XmlDocument xDoc = new XmlDocument();
		Debug.Log (Application.dataPath);
		xDoc.Load("./Exercises/" + name);
		Debug.Log(name);
		
		XmlNodeList exer = xDoc.GetElementsByTagName("EXERCISE");	  
		artIni = Convert.ToInt16(exer[0].Attributes["initialId"].InnerText);
		artEnd = Convert.ToInt16(exer[0].Attributes["finalId"].InnerText);
		
		XmlNodeList angles = xDoc.GetElementsByTagName("ang");
		XmlNodeList vector = xDoc.GetElementsByTagName("axis");
		XmlNodeList pos0 = xDoc.GetElementsByTagName("ini");
		XmlNodeList rotI = xDoc.GetElementsByTagName("rotIni");
		XmlNodeList rotE = xDoc.GetElementsByTagName("rotEnd");
		XmlNodeList reference = xDoc.GetElementsByTagName ("reference");
		XmlNodeList restrictions = ((XmlElement)exer[0]).GetElementsByTagName("Restrictions");
		
		//Angulos maximo y minimo de ejercicio
		minimo = Convert.ToInt16(angles[0].Attributes["min"].InnerText);
		maximo = Convert.ToInt16(angles[0].Attributes["max"].InnerText);
		
		//plano sobre el que se va a realizar la medicion
		plane.SetX(Convert.ToInt16(vector[0].Attributes["x"].InnerText));
		plane.SetY(Convert.ToInt16(vector[0].Attributes["y"].InnerText));
		plane.SetZ(Convert.ToInt16(vector[0].Attributes["z"].InnerText));
		
		//posicion de inicio del ejercicio
		initBone.SetX(Convert.ToInt16(pos0[0].Attributes["x"].InnerText));
		initBone.SetY(Convert.ToInt16(pos0[0].Attributes["y"].InnerText));
		initBone.SetZ(Convert.ToInt16(pos0[0].Attributes["z"].InnerText));
		
		//Rotacion inicial de la articulacion principal
		rotIni.x = Convert.ToInt16(rotI[0].Attributes["x"].InnerText);
		rotIni.y = Convert.ToInt16(rotI[0].Attributes["y"].InnerText);
		rotIni.z = Convert.ToInt16(rotI[0].Attributes["z"].InnerText);
		
		//Rotacion final de la articulacion principal
		rotEnd.x = Convert.ToInt16(rotE[0].Attributes["x"].InnerText);
		rotEnd.y = Convert.ToInt16(rotE[0].Attributes["y"].InnerText);
		rotEnd.z = Convert.ToInt16(rotE[0].Attributes["z"].InnerText);
		
		/*	
		refId = Convert.ToInt16(reference[0].Attributes["id"].InnerText);
		referenceArt.SetX(Convert.ToInt16(reference[0].Attributes["x"].InnerText));
		referenceArt.SetY(Convert.ToInt16(reference[0].Attributes["y"].InnerText));
		referenceArt.SetZ(Convert.ToInt16(reference[0].Attributes["z"].InnerText));
	*/		
		
		XmlNodeList ID;
		XmlNodeList ID1;		
		XmlNodeList FX;
		XmlNodeList FY;
		XmlNodeList FZ;
		XmlNodeList RX;
		XmlNodeList RY;
		XmlNodeList RZ;
		XmlNodeList G;
		
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
				Vector aux = new Vector();
				aux.SetX(Convert.ToInt16(FX[i].InnerText));
				aux.SetY(Convert.ToInt16(FY[i].InnerText));
				aux.SetZ(Convert.ToInt16(FZ[i].InnerText));
				pose.SetBone(aux);
				
				pose.RotIni = new Vector3(Convert.ToInt16(RX[i].InnerText),
				                          Convert.ToInt16(RY[i].InnerText),
				                          Convert.ToInt16(RZ[i].InnerText));
				
				//define las restricciones en angulos con respecto a la posicion correcta
				pose.SetGrado(Convert.ToInt16(G[i].InnerText));
				
				//Lista de restricciones del ejercicio
				poseList.Add (pose);
				i++; 
			}
		}
		
		//Cargamos las esferas en las articulaciones principales
		loadSphere(IniSphere, artIni, "IniSphere");
		loadSphere(EndSphere, artEnd, "EndSphere");
		
		//Se ejecuta el movimiento
		ShowMovement();
	}














}
