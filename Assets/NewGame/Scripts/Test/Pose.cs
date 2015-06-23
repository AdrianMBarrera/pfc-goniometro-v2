using UnityEngine;

public class Pose {
	
	private int art; 
	private int art1;
	public Vector3 bone; // posicion correcta del hueso
	public float grado; // restriccion en grados
	private Vector3 reposePos;
	private Vector3 rotIni;




	public Pose() {
		art = 0; art1 = 0; grado = 0;
		bone = new Vector3();
	}		
	
	public void SetArt (int x) {art = x;}
	public void SetArt1 (int y) {art1 = y;}
	public void SetBone (Vector3 v) {bone = v;}
	public void SetGrado (float x) {grado = x;}
	
	public int GetArt() {return art;}
	public int GetArt1() {return art1;}
	public Vector3 GetBone() {return bone;}
	public float GetGrado() {return grado;}


	public Vector3 ReposePos{
		set{reposePos = value;}
		get{return reposePos;}
	}

	public Vector3 RotIni{
		set{rotIni = value;}
		get{return rotIni;}
	}

	public int Art{
		set{art = value;}
		get{return art;}
	}

	public int Art1{
		set{art1 = value;}
		get{return art1;}
	}

	public Vector3 Bone{
		set{bone = value;}
		get{return bone;}
	}



}
