using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioClip musicClip;

	public AudioClip goodClip;

	public AudioClip wrongClip;

	public AudioClip excellentClip;

	public AudioSource feedbackMusic;

	public AudioSource music;


	void OnEnable(){
		GameManager.OnCheckFeedBack += SetFeedBack;
	}
	
	void OnDisable(){
		GameManager.OnCheckFeedBack -= SetFeedBack;
	}



	void SetFeedBack(){

		if (GameManager.instance.maxAngle >= 90){

			feedbackMusic.clip = excellentClip;


		}else{ if  (GameManager.instance.maxAngle >= 65){
				feedbackMusic.clip = goodClip;

			}else{
				feedbackMusic.clip = wrongClip;

			}
		}

		feedbackMusic.Play();


	}





	// Use this for initialization
	void Start () {
		//music.clip = musicClip;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Camera.main.transform.position;
	}
}
