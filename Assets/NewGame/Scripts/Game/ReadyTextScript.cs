using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadyTextScript : MonoBehaviour {

	private Animator readyAnim;

	void OnEnable(){
		
		GameManager.OnInGamePhase += ReadyAnimation;
		
	}



	// Use this for initialization
	void Start () {
		readyAnim = GetComponent<Animator>();
		

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void ReadyAnimation(){
	
		readyAnim.Play("Idle");
		readyAnim.SetBool("isBig", true);
	}





}
