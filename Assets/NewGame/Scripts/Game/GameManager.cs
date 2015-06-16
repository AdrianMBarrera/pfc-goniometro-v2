using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	ZigEngageSingleUser zesu;
	public GameObject zigfu;

	public enum statesOfGame : int {None = -1,Calibration = 0 , LoadGame = 1, Demostration = 2,  InGame = 3 , End = 4 }

	[Tooltip("Temporizador del tiempo de juego")]
	public float timer;
	
	[Tooltip("estado en el que se encuentra el juego 0.- Calibrado, 1.- Cargando, 2.- Demostracion del ejercicio," +
		"3.- En Ejecucion, 4.-Final")]
	public statesOfGame stateOfGame = statesOfGame.None;


	[Tooltip("Posicion en la lista del ejercicio actual que se esta ejecutando. Valor -1 al inicio.")]
	public int currentExercise = -1;

	public float TotalTime = 0;

	public InfoPlayer.gameModes gameMode = InfoPlayer.gameModes.None;

	//Variables para la medicion del ejercicio
	ZigSkeleton zs;
	public int artIni, artEnd = 0;
	public float minimo, maximo;
	public Vector3 plane, initBone;
	public List<Pose> poseList = new List<Pose>();  //Lista de restricciones a tener en cuenta durante el ejercicio



	public delegate void LoadGamePhase();
	public static event LoadGamePhase OnLoadGamePhase;

	public delegate void DemostrationPhase();
	public static event DemostrationPhase OnDemostrationPhase;

	public delegate void CalibrationPhase();
	public static event CalibrationPhase OnCalibrationPhase;

	public delegate void InGamePhase();
	public static event InGamePhase OnInGamePhase;
	
	public delegate void EndPhase();
	public static event EndPhase OnEndPhase;







	
	void Awake(){

		instance = this;

	}



	// Use this for initialization
	void Start () {


		InfoPlayer.gameModes mode = (InfoPlayer.gameModes) gameMode;

		mode = (InfoPlayer.gameModes.Open) ;  //ACORDARSE DE QUITAR ESTO

		switch(mode){

		case(InfoPlayer.gameModes.Open): break;


		case (InfoPlayer.gameModes.Custom) : break;


		case (InfoPlayer.gameModes.Preset) : break;



		}

	
	}
	
	// Update is called once per frame
	void Update () {




	}


	//Se lanza cuando estas calibrado para empezar el ejercicio

	public void DoExercise(){

		stateOfGame = statesOfGame.InGame;

		if (OnInGamePhase != null){

			OnInGamePhase();

		}

	}

	//se llama para lanzar al dummy para mostrar el ejercicio que tieens que realizar

	public void NextExercise() {


		currentExercise++;
		if (GameManager.instance.currentExercise < InfoPlayer.alExercise.Count) {
			stateOfGame = statesOfGame.LoadGame;
			DummyManager.instance.LoadXml(InfoPlayer.alExercise[currentExercise].FileName);
			if (OnDemostrationPhase != null) {
				OnDemostrationPhase();
			}
		}
		else {
			stateOfGame = statesOfGame.End;
			if (OnEndPhase != null) {
				OnEndPhase();
			}
		}



	}





	
	IEnumerator WaitForBegin(){
		
		yield return new WaitForSeconds (2f);
		zigfu.SetActive(true);
		NextExercise();
	}

	// se llama la primera vez que empieza el juego

	public void OnLoad() {

		if (OnLoadGamePhase != null){
			OnLoadGamePhase();
		}

		StartCoroutine(WaitForBegin());
	}

	public void Calibrate() {

		stateOfGame = 0;
		if (OnCalibrationPhase != null){
			OnCalibrationPhase();
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

	public float CalcAngle (Vector3 bone, Vector3 initBone) {
		float scalarProduct = bone.x * initBone.x + 
			bone.y * initBone.y + 
				bone.z * initBone.z;
		
		float ModuleBone = Mathf.Sqrt(bone.x * bone.x + 
		                                     bone.y * bone.y + 
		                                     bone.z * bone.z);
		
		float ModuleInitBone = Mathf.Sqrt(initBone.x * initBone.x + 
		                                         initBone.y * initBone.y + 
		                                         initBone.z * initBone.z);
		
		float cos = scalarProduct / (ModuleBone * ModuleInitBone);
		
		float ang = Mathf.Acos(cos) * 180f / Mathf.PI; //se pasa de radianes a grados
		
		return ang;	
	}
	
	//Calculo del producto vectorial de dos vectores dados
	public Vector3 ProdVec(Vector3 a, Vector3 b){
		
		Vector3 resultado = new Vector3();
		
		resultado = new Vector3 (a.y * b.z - a.z * b.y,
		                         a.z * b.x - a.x * b.z,
		                         a.x * b.y - a.y * b.x);
		
		return resultado;
	}
	
	//calcula el angulo que forman las proyecciones de dos vectores en un plano dado.
	public float AngleProjection (Vector3 bone, Vector3 plane, Vector3 initBone) {
		
		Vector3 productVect = new Vector3();
		Vector3 proyectBone = new Vector3();
		Vector3 proyectBone1 = new Vector3();
		Vector3 productVect1 = new Vector3();
		
		productVect = ProdVec(bone, plane);
		proyectBone = ProdVec(plane, productVect); //proyeccion sobre el plano del vector a medir
		
		productVect = ProdVec(initBone, plane);
		proyectBone1 = ProdVec(plane, productVect);// proyeccion sobre el plano del vector de inicio
		
		float scalarProduct = proyectBone.x * proyectBone1.x +
			proyectBone.y * proyectBone1.y +
				proyectBone.z * proyectBone1.z;
		
		float ModuleProyect = Mathf.Sqrt(proyectBone.x * proyectBone.x +
		                                 proyectBone.y * proyectBone.y + 
		                                 proyectBone.z * proyectBone.z);
		
		float ModuleProyect1 = Mathf.Sqrt(proyectBone1.x * proyectBone1.x + 
		                                  proyectBone1.y * proyectBone1.y + 
		                                  proyectBone1.z * proyectBone1.z);
		
		float cos = scalarProduct / (ModuleProyect * ModuleProyect1);
		
		float angulo = Mathf.Acos(cos) * 180f / Mathf.PI; //se pasa de radianes a grados
		
		productVect1 = ProdVec(plane, proyectBone);
		float scalarProduct1 = proyectBone1.x * productVect1.x + proyectBone1.y * productVect1.y + proyectBone1.z * productVect1.z;		
		
		if (scalarProduct1 < 0) angulo = -angulo;
		
		return angulo;	
	}

	public void Medir() {       
		Transform aux = art(artIni);
		Transform aux1 = art(artEnd);
		
		Vector3 joint = new Vector3 (aux.position.x, aux.position.y, aux.position.z);
//		joint.SetName(aux.ToString());
		
		Vector3 joint1 = new Vector3 (aux.position.x, aux.position.y, aux.position.z);
//		joint1.SetName(aux1.ToString());
		
		//Representa el hueso de la articulacion a medir
		Vector3 bone = new Vector3(distance(joint1.x, joint.x), distance(joint1.y, joint.y), distance(joint1.z, joint.z));
//		bone.SetName(joint.GetName());
		
		//calcula las restricciones
		//restricciones();
		
		float ang = AngleProjection(bone, plane, initBone);
//		texto.text = (Math.Truncate(ang)).ToString();
//		if (Interfaz == true) {
//			textoMin.text = "Minimo: ";
//			texto1.text = Math.Truncate(medicion.GetMinGlobal()).ToString();
//			textoMax.text = "Maximo: ";	
//			texto2.text = Math.Truncate(medicion.GetMaxGlobal()).ToString();
//			textoRep.text = "Repeticiones: ";
//			texto3.text = Math.Truncate(medicion.GetRepeticiones()).ToString();
//			textoDificultad.text = "Dificultad(%): ";
//			texto4.text = dificultad.ToString();
//		}
//		
//		if (barra) {	
//			Barra.guiTexture.pixelInset = new Rect(-64,-29,(((float)ang * 400)/ maximo-minimo), 18);
//			if ((((float)ang * 400)/ maximo-minimo) < (60 * 400 /100))
//				Barra.guiTexture.texture = bajo;
//			else if (((((float)ang * 400)/ maximo-minimo) > (60 * 400/100)) && (((float)ang * 400)/ maximo-minimo < (80 * 400 /100))) 
//				Barra.guiTexture.texture = medio;
//			else
//				Barra.guiTexture.texture = alto;
//		}
		
		//tick es el tiempo de juego transcurrido y cada vez que pase el limite escribe resutaldos y se resetea
//		if (medicion.GetTick() >= medicion.GetLimit()) {	
//			//globales
//			if (ang < medicion.GetMinGlobal())
//				medicion.SetMinGlobal(ang);
//			else if (ang > medicion.GetMaxGlobal())
//				medicion.SetMaxGlobal(ang);
//			
//			//calcula la dificultad del ejercicio
//			float nivel = ((maximo - minimo)-(dificultad * (maximo - minimo) / 100));
//			//locales->deteccion de repeticion
//			if(ang > medicion.GetMaxLocal())
//				medicion.SetMaxLocal(ang);
//			else if (((medicion.GetMaxLocal() - ang) > 5) && (ang < medicion.GetMaxLocal()) && ((maximo - medicion.GetMaxLocal()) < nivel) && (medicion.GetMaxLocal() != 0)){
//				medicion.SetRepeticiones(medicion.GetRepeticiones() + 1);
//				medicion.SetMaxLocal(0);
//				medicion.Guardar(bone.GetName(), medicion.GetMinGlobal(), medicion.GetMaxGlobal(), ang, plane);
//			}
//			medicion.SetTick(0);
//		}
	}


}
