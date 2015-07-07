using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


	// VARIABLES

	public static GameManager instance;

	public GameObject restrictSphere;
	public GameObject[] rsArray;
	public GameObject zigfu;

	public enum statesOfGame : int {None = -1,Calibration = 0 , LoadGame = 1, Demostration = 2,  InGame = 3 , End = 4 }

	[Tooltip("Temporizador del tiempo de ejercicio")]
	public float timer = 0;
	
	[Tooltip("estado en el que se encuentra el juego 0.- Calibrado, 1.- Cargando, 2.- Demostracion del ejercicio," +
		"3.- En Ejecucion, 4.-Final")]
	public statesOfGame stateOfGame = statesOfGame.None;


	[Tooltip("Posicion en la lista del ejercicio actual que se esta ejecutando. Valor -1 al inicio.")]
	public int currentExercise = -1;

	[Tooltip("Temporizador del tiempo de juego")]
	public float TotalTime = 0;

	[Tooltip("Tamaño 15.\n0. Head\n1. Neck\n2. Spine1\n3. JointLeftArm\n4. JointLeftForeArm\n5. JointLeftHand\n" +
	         "6. JointRightArm\n7. JointRightForeArm\n8. JointRightHand\n9. LeftUpLeg\n10. JointLeftLeg\n11. LeftFoot\n" +
	         "12. RightUpLeg\n13. JointRightLeg\n14. RightFoot")]
	public GameObject[] articulations;

	//public InfoPlayer.gameModes gameMode = InfoPlayer.gameModes.None;

	public  GameObject player;



	// ----- Variables para la medicion del ejercicio
	public int artIni, artEnd = 0;
	public float minimo, maximo;
	public Vector3 plane, initBone;
	public List<Pose> poseList;  //Lista de restricciones a tener en cuenta durante el ejercicio

	public float angle = 0;
	public float maxAngle = 0;

	private bool isRep = true;

	public bool waitForPlayer = false;


	//----EVENTOS


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


	public delegate void CheckFeedBack();
	public static event CheckFeedBack OnCheckFeedBack;




	void OnEnable(){
		DummyManager.OnEndExercise += ActivateZigfu;

	}

	void OnDisable(){

		DummyManager.OnEndExercise -= ActivateZigfu;

	}


	
	void Awake(){

		instance = this;

	}

	// Use this for initialization
	void Start () {

		poseList = new List<Pose>();

		//InfoPlayer.gameModes mode = InfoPlayer.gameMode;

		switch(InfoPlayer.gameMode){

			case(InfoPlayer.gameModes.Open): 
				break;

			case (InfoPlayer.gameModes.Custom) : 
				break;

			case (InfoPlayer.gameModes.Preset) :
				break;
		}
	
	}

	void LoadStats() {
		if (stateOfGame == statesOfGame.End) {
			Destroy(GameObject.Find("ZigInputContainer"));
			Destroy(GameObject.Find ("Zigfu"));
			GameObject.Find("Veil").GetComponent<LoadingScript>().BeginLevel("GameStats");
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch(InfoPlayer.gameMode){	
			case(InfoPlayer.gameModes.Open): 
				break;
				
			case (InfoPlayer.gameModes.Custom) : 
				LoadStats();
				break;
				
			case (InfoPlayer.gameModes.Preset) :
				LoadStats();
				break;
		}
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
			stateOfGame = statesOfGame.Demostration;
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


	IEnumerator waitForPlayerCoroutine() {
		yield return new WaitForSeconds(2f);
		waitForPlayer = true;
	}


	void ActivateZigfu(){
		if (stateOfGame != statesOfGame.None){

			GameObject zigInput = GameObject.Find("ZigInputContainer");
			if (zigInput){
				Destroy (GameObject.Find("Zigfu"));

	
			}
			zigfu = Instantiate(zigfu) as GameObject;
			zigfu.name = "Zigfu";
			ZigEngageSingleUser zesu = zigfu.GetComponent<ZigEngageSingleUser>();
			ZigDepthViewer zdv =  zigfu.GetComponent<ZigDepthViewer>();
			ZigUsersRadar zur =  zigfu.GetComponent<ZigUsersRadar>();
			zesu.EngagedUsers[0] = player;
			zesu.enabled = true;
			zdv.enabled = true;
			zur.enabled = true;

			waitForPlayer = false;
			StartCoroutine(waitForPlayerCoroutine());

		}

	}













	void SaveStats (){

		Debug.Log ("CURRENT EXERCISE: " + currentExercise);

		if (maxAngle > InfoPlayer.alExercise[currentExercise].Max){

			InfoPlayer.alExercise[currentExercise].Max = maxAngle;
		}

		if (maxAngle < InfoPlayer.alExercise[currentExercise].Min){

			InfoPlayer.alExercise[currentExercise].Min = maxAngle;
		}


		if  (GameManager.instance.maxAngle >= 65){

			InfoPlayer.alExercise[currentExercise].Success +=1;
			
		}else{
			InfoPlayer.alExercise[currentExercise].Fail +=1;
			
		}
		//timer += TotalTime;
		InfoPlayer.alExercise[currentExercise].Duration = timer;



	}















	//--------------------------------------------------------------------------







	//Traductor entre los joints del fichero de definicion y el skeleton de kinect
	public Transform art(int op) {
		//Debug.Log ("Articulacion" + op);
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
		//Debug.Log ("Proyect Bone: " + proyectBone);
		
		productVect = ProdVec(initBone, plane);
		proyectBone1 = ProdVec(plane, productVect);// proyeccion sobre el plano del vector de inicio
		
		float scalarProduct = proyectBone.x * proyectBone1.x +
			proyectBone.y * proyectBone1.y +
				proyectBone.z * proyectBone1.z;
		
		float ModuleProyect = Mathf.Sqrt(proyectBone.x * proyectBone.x +
		                                 proyectBone.y * proyectBone.y + 
		                                 proyectBone.z * proyectBone.z);

		//Debug.Log ("peppe: " + proyectBone.x);
		
		float ModuleProyect1 = Mathf.Sqrt(proyectBone1.x * proyectBone1.x + 
		                                  proyectBone1.y * proyectBone1.y + 
		                                  proyectBone1.z * proyectBone1.z);
		//Debug.Log ("Module:" + ModuleProyect);
		//Debug.Log ("Module1: " + ModuleProyect1);
		
		float cos = scalarProduct / (ModuleProyect * ModuleProyect1);
		
		float angulo = Mathf.Acos(cos) * 180f / Mathf.PI; //se pasa de radianes a grados
		
		productVect1 = ProdVec(plane, proyectBone);
		float scalarProduct1 = proyectBone1.x * productVect1.x + proyectBone1.y * productVect1.y + proyectBone1.z * productVect1.z;		
		
		if (scalarProduct1 < 0) angulo = -angulo;
		return angulo;	
	}

	public void restricciones() {
		//restricciones definidas para cada ejercicio


		int i = 0;
		foreach (Pose pose in poseList) {
			
			Transform aux = art(pose.GetArt());
			Transform aux1 = art(pose.GetArt1());
			
			Vector3 bone1 = new Vector3(distance(aux1.position.x, aux.position.x), 
			                            distance(aux1.position.y, aux.position.y),
			                            distance(aux1.position.z, aux.position.z));
			
			//texto.text = bone1.GetX().ToString()+" "+bone1.GetY().ToString()+" "+bone1.GetZ().ToString();
			if (CalcAngle(bone1, pose.GetBone()) < pose.GetGrado()) {
				rsArray[i].renderer.material.color = new Color(0, 255f, 0, 100f);
			}
			else {
				rsArray[i].renderer.material.color = new Color(255f, 0, 0, 100f);
			}
			i++;
		}
	}

	public void Medir() {   

		//float temp = angle; //variable para ver si la medicion aumenta o no
		if (maxAngle < MathUtils.AngToPercent(angle))
			maxAngle = MathUtils.AngToPercent(angle);

		Transform aux = art(artIni);
		Transform aux1 = art(artEnd);
		
		Vector3 joint = new Vector3 (aux.position.x, aux.position.y, aux.position.z);
		Vector3 joint1 = new Vector3 (aux1.position.x, aux1.position.y, aux1.position.z);

		//Representa el hueso de la articulacion a medir
		Vector3 bone = new Vector3(distance(joint1.x, joint.x), distance(joint1.y, joint.y), distance(joint1.z, joint.z));
		
		//calcula las restricciones
		restricciones();
		angle = AngleProjection(bone, plane, initBone);

		if (isRep) {
			if (maxAngle-15 > MathUtils.AngToPercent(angle)){
				SaveStats();
				if (OnCheckFeedBack != null){
					OnCheckFeedBack();
				}
				isRep = false;
			}
		}
		if (MathUtils.AngToPercent(angle) < 10) {
			maxAngle = MathUtils.AngToPercent(angle);
			isRep = true;
		}


		//Debug.Log ("Angulo actual: " + ang);
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







}//end class
