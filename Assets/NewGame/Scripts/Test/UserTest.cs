using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System.Xml;

public class UserTest : MonoBehaviour {

	ZigSkeleton zs;

	public bool plano = true;
	public bool barra = true;	
	public int arti, arti1, inicio, eje = 0;
	
	public bool Interfaz = false;
	public GUIText texto;
	public GUIText textoMin;
	public GUIText texto1; // minimo
	public GUIText textoMax;
	public GUIText texto2; //maximo
	public GUIText textoRep;
	public GUIText texto3; //repeticiones
	public GUIText textoDificultad; // indica la dificultad del ejercicio;
	public GUIText texto4;
	public Texture2D bajo, medio, alto;
	public GameObject Plano, Barra;
	
	public float dificultad = 50; // indica el porcentaje de recorrido que se debe realizar para que se cuente como repeticion
	Timer timer;
	
	Resultados medicion = new Resultados();
	
	public float minimo, maximo; // maximo y minimo angulo del ejercicio

	Vector plane = new Vector(); // plano de medicion->definido en el fichero de definiciones
	Vector initBone = new Vector(); //posicion inicial del brazo, con respecto a esta posicion se medira

	List<Pose> poseList = new List<Pose>();  //Lista de articulaciones a tener en cuenta durante el ejercicio 


	// Use this for initialization
	void Start () {
		zs = gameObject.GetComponent<ZigSkeleton>();
		
		Plano = GameObject.CreatePrimitive(PrimitiveType.Plane);
		Plano.name = "plano";
		Plano.transform.localScale = new Vector3(0.09f,0,0.09f);
		Plano.renderer.material.shader = Shader.Find("Transparent/Diffuse");
		Plano.renderer.material.color = new Color(0,1,0,0.4f);
		Plano.renderer.enabled = plano; //visibilidad
		
		Barra = GameObject.Find("barra");
		Barra.renderer.enabled = barra;
	}

	
	//carga los datos de un fichero de definiciones de ejercicios
	public void Cargar() {
		
		XmlDocument xDoc = new XmlDocument();
		Debug.Log (Application.dataPath);
		xDoc.Load("./ejer.xml");
		
		XmlNodeList exer = xDoc.GetElementsByTagName("EXERCISE");	  
		arti = Convert.ToInt16(exer[0].Attributes["initialId"].InnerText);
		arti1 = Convert.ToInt16(exer[0].Attributes["finalId"].InnerText);
		
		XmlNodeList angles = xDoc.GetElementsByTagName("ANGLE");
		XmlNodeList vector = xDoc.GetElementsByTagName("AXIS");
		XmlNodeList pos0 = xDoc.GetElementsByTagName("INI");
		XmlNodeList frames = ((XmlElement)exer[0]).GetElementsByTagName("RESTRICTION");
		
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
		
		XmlNodeList ID;
		XmlNodeList ID1;		
		XmlNodeList FX;
		XmlNodeList FY;
		XmlNodeList FZ;
		XmlNodeList G;
		
		foreach (XmlElement frame in frames) {
			
			int i = 0;
			Pose pose = new Pose();	
			ID = frame.GetElementsByTagName("initialId");
			ID1 = frame.GetElementsByTagName("finalId");
			FX = frame.GetElementsByTagName("x");
			FY = frame.GetElementsByTagName("y");
			FZ = frame.GetElementsByTagName("z");
			G = frame.GetElementsByTagName("grade");
			
			//define el hueso que vamos a tener en cuenta
			pose.SetArt(Convert.ToInt16(ID[i].InnerText));
			pose.SetArt1(Convert.ToInt16(ID1[i].InnerText));
			
			//define la posicion correcta para el ejercicio
			Vector aux = new Vector();
			aux.SetX(Convert.ToInt16(FX[i].InnerText));
			aux.SetY(Convert.ToInt16(FY[i].InnerText));
			aux.SetZ(Convert.ToInt16(FZ[i].InnerText));
			pose.SetBone(aux);
			
			//define las restricciones en angulos con respecto a la posicion correcta
			pose.SetGrado(Convert.ToInt16(G[i].InnerText));
			
			//Lista de restricciones del ejercicio
			poseList.Add (pose);
			i++; 
		}
		
	}


	//Traductor entre los joints del fichero de definicion y el skeleton de kinect
	public Transform art(int op) {
		switch (op) {
			case 1:	return zs.Head;
			case 2: return zs.Neck;
			case 3: return zs.Torso;
			case 4: return zs.Waist;
			case 5: return zs.LeftCollar;
			case 6: return zs.LeftShoulder;
			case 7: return zs.LeftElbow;
			case 8: return zs.LeftWrist;
			case 9: return zs.LeftHand;
			case 10: return zs.LeftFingertip;
			case 11: return zs.RightCollar;
			case 12: return zs.RightShoulder;
			case 13: return zs.RightElbow;
			case 14: return zs.RightWrist;
			case 15: return zs.RightHand;
			case 16: return zs.RightFingertip;
			case 17: return zs.LeftHip;
			case 18: return zs.LeftKnee;
			case 19: return zs.LeftAnkle;
			case 20: return zs.LeftFoot;
			case 21: return zs.RightHip;
			case 22: return zs.RightKnee;
			case 23: return zs.RightAnkle;
			case 24: return zs.RightFoot;
			default: return zs.RightKnee;
		}
	}
	
	float distance(float right, float left) {
		if ((right < 0 && left < 0) || (right > 0 && left > 0)) return (right - left);
		else return (right + left);
	}

	public double CalcAngle (Vector bone, Vector initBone) {
		double scalarProduct = bone.GetX() * initBone.GetX() + 
			                   bone.GetY() * initBone.GetY() + 
				               bone.GetZ() * initBone.GetZ();

		double ModuleBone = System.Math.Sqrt(bone.GetX() * bone.GetX() + 
		                                     bone.GetY() * bone.GetY() + 
		                                     bone.GetZ() * bone.GetZ());

		double ModuleInitBone = System.Math.Sqrt(initBone.GetX() * initBone.GetX() + 
		                                         initBone.GetY() * initBone.GetY() + 
		                                         initBone.GetZ() * initBone.GetZ());

		double cos = scalarProduct / (ModuleBone * ModuleInitBone);

		double ang = System.Math.Acos(cos) * 180 / System.Math.PI; //se pasa de radianes a grados
		
		return ang;	
	}

	//Calculo del producto vectorial de dos vectores dados
	public Vector ProdVec(Vector a, Vector b){
		
		Vector resultado = new Vector();
		
		resultado.SetX(a.GetY() * b.GetZ() - a.GetZ() * b.GetY());
		resultado.SetY(a.GetZ() * b.GetX() - a.GetX() * b.GetZ());
		resultado.SetZ(a.GetX() * b.GetY() - a.GetY() * b.GetX());
		
		return resultado;
	}
	
	//calcula el angulo que forman las proyecciones de dos vectores en un plano dado.
	public double AngleProjection (Vector bone, Vector plane, Vector initBone) {
		
		Vector productVect = new Vector();
		Vector proyectBone = new Vector();
		Vector proyectBone1 = new Vector();
		Vector productVect1 = new Vector();
		
		productVect = ProdVec(bone, plane);
		proyectBone = ProdVec(plane, productVect); //proyeccion sobre el plano del vector a medir
		
		productVect = ProdVec(initBone, plane);
		proyectBone1 = ProdVec(plane, productVect);// proyeccion sobre el plano del vector de inicio
		
		double scalarProduct = proyectBone.GetX() * proyectBone1.GetX() +
			                   proyectBone.GetY() * proyectBone1.GetY() +
				               proyectBone.GetZ() * proyectBone1.GetZ();

		double ModuleProyect = Math.Sqrt(proyectBone.GetX() * proyectBone.GetX() +
		                                 proyectBone.GetY() * proyectBone.GetY() + 
		                                 proyectBone.GetZ() * proyectBone.GetZ());

		double ModuleProyect1 = Math.Sqrt(proyectBone1.GetX() * proyectBone1.GetX() + 
		                                  proyectBone1.GetY() * proyectBone1.GetY() + 
		                                  proyectBone1.GetZ() * proyectBone1.GetZ());

		double cos = scalarProduct / (ModuleProyect * ModuleProyect1);
		
		double angulo = Math.Acos(cos) * 180 / System.Math.PI; //se pasa de radianes a grados
		
		productVect1 = ProdVec(plane, proyectBone);
		double scalarProduct1 = proyectBone1.GetX() * productVect1.GetX() + proyectBone1.GetY() * productVect1.GetY() + proyectBone1.GetZ() * productVect1.GetZ();		
		
		if (scalarProduct1 < 0) angulo = -angulo;
		
		return angulo;	
	}
	
	public void restricciones() {
		//restricciones definidas para cada ejercicio
		foreach (Pose pose in poseList) {
			
			Transform aux = art(pose.GetArt());
			Transform aux1 = art(pose.GetArt1());
			
			Vector bone1 = new Vector();
			bone1.SetX(distance(aux1.position.x, aux.position.x));
			bone1.SetY(distance(aux1.position.y, aux.position.y));
			bone1.SetZ(distance(aux1.position.z, aux.position.z));
			bone1.SetName(aux.ToString());
			
			//texto.text = bone1.GetX().ToString()+" "+bone1.GetY().ToString()+" "+bone1.GetZ().ToString();
			if (CalcAngle(bone1, pose.GetBone()) < pose.GetGrado())
				aux.renderer.material.color = Color.green;
			else
				aux.renderer.material.color = Color.red;
		}
	}
	
	public void Medir() {       
		Transform aux = art(arti);
		Transform aux1 = art(arti1);
		
		Vector joint = new Vector();
		joint.SetX(aux.position.x);
		joint.SetY(aux.position.y);
		joint.SetZ(aux.position.z);
		joint.SetName(aux.ToString());
		
		Vector joint1 = new Vector();
		joint1.SetX(aux1.position.x);
		joint1.SetY(aux1.position.y);
		joint1.SetZ(aux1.position.z);
		joint1.SetName(aux1.ToString());
		
		//Representa el hueso de la articulacion a medir
		Vector bone = new Vector();
		bone.SetX(distance(joint1.GetX(), joint.GetX()));
		bone.SetY(distance(joint1.GetY(), joint.GetY()));
		bone.SetZ(distance(joint1.GetZ(), joint.GetZ()));
		bone.SetName(joint.GetName());
		
		//dibuja el plano sobre el que se va a medir
		if (plano) {
			Plano.transform.position = new Vector3(joint.GetX(), joint.GetY(), joint.GetZ());
			//Plano.transform.rotation = Quaternion.AngleAxis(90, new Vector3(plane.GetX(),plane.GetY(),plane.GetZ()));
			if(plane.GetZ() != 0)
				Plano.transform.eulerAngles = new Vector3(270,0,0);
			else if (plane.GetY () != 0)
				Plano.transform.eulerAngles = new Vector3(0,0,0);
			else if (plane.GetX () != 0)
				Plano.transform.eulerAngles = new Vector3(270,270,0);
		}
		
		//calcula las restricciones
		restricciones();
		
		double ang = AngleProjection(bone, plane, initBone);
		texto.text = (Math.Truncate(ang)).ToString();
		//texto.text = bone.GetX().ToString()+" "+bone.GetY().ToString()+" "+bone.GetZ().ToString();
		if (Interfaz == true) {
			textoMin.text = "Minimo: ";
			texto1.text = Math.Truncate(medicion.GetMinGlobal()).ToString();
			textoMax.text = "Maximo: ";	
			texto2.text = Math.Truncate(medicion.GetMaxGlobal()).ToString();
			textoRep.text = "Repeticiones: ";
			texto3.text = Math.Truncate(medicion.GetRepeticiones()).ToString();
			textoDificultad.text = "Dificultad(%): ";
			texto4.text = dificultad.ToString();
		}
		
		if (barra) {	
			Barra.guiTexture.pixelInset = new Rect(-64,-29,(((float)ang * 400)/ maximo-minimo), 18);
			if ((((float)ang * 400)/ maximo-minimo) < (60 * 400 /100))
				Barra.guiTexture.texture = bajo;
			else if (((((float)ang * 400)/ maximo-minimo) > (60 * 400/100)) && (((float)ang * 400)/ maximo-minimo < (80 * 400 /100))) 
				Barra.guiTexture.texture = medio;
			else
				Barra.guiTexture.texture = alto;
		}
		
		//tick es el tiempo de juego transcurrido y cada vez que pase el limite escribe resutaldos y se resetea
		medicion.SetTick(medicion.GetTick() + Time.deltaTime);
		
		if (medicion.GetTick() >= medicion.GetLimit()) {	
			//globales
			if (ang < medicion.GetMinGlobal())
				medicion.SetMinGlobal(ang);
			else if (ang > medicion.GetMaxGlobal())
				medicion.SetMaxGlobal(ang);
			
			//calcula la dificultad del ejercicio
			float nivel = ((maximo - minimo)-(dificultad * (maximo - minimo) / 100));
			//locales->deteccion de repeticion
			if(ang > medicion.GetMaxLocal())
				medicion.SetMaxLocal(ang);
			else if (((medicion.GetMaxLocal() - ang) > 5) && (ang < medicion.GetMaxLocal()) && ((maximo - medicion.GetMaxLocal()) < nivel) && (medicion.GetMaxLocal() != 0)){
				medicion.SetRepeticiones(medicion.GetRepeticiones() + 1);
				medicion.SetMaxLocal(0);
				medicion.Guardar(bone.GetName(), medicion.GetMinGlobal(), medicion.GetMaxGlobal(), ang, plane);
			}
			medicion.SetTick(0);
		}
	}
}
