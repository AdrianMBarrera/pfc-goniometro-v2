using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour {
	
	private Image FadeTexture;
	public float fadeSpeed = 1f;
	private float alpha = 1f;
	private int fadeDir = -1;
	// Use this for initialization
	void Start () {
		FadeTexture = GetComponent<Image>();

	}

	void Update(){

		alpha+= fadeDir*fadeSpeed*Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);
		FadeTexture.color =  new Color (FadeTexture.color.r, FadeTexture.color.g, FadeTexture.color.b, alpha);
		//GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);

	}


	public float BeginFade(int dir){
		fadeDir = dir;
		return fadeSpeed;

	}


	public void BeginLevel(string level){
		StartCoroutine(BeginLevelCoroutine(level));

	}


	 IEnumerator BeginLevelCoroutine (string level){
		float fadeTime = BeginFade(1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel(level);
		
	}



	public void Exit(){

		Application.Quit();

	}


}
