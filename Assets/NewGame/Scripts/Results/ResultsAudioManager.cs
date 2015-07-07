using UnityEngine;
using System.Collections;

public class ResultsAudioManager : MonoBehaviour {

	public AudioClip startClip;

	public AudioClip textClip;
	
	public AudioClip progressBarClip;

	public AudioClip finalBadClip;

	public AudioClip finalRegularClip;

	public AudioClip finalGoodClip;

	public AudioClip finalPerfectClip;
	
	public AudioSource fx;
	
	
	void OnEnable(){
		ResultManager.OnLoadStartClip += SetStartClip;
		ResultManager.OnLoadTextClip += SetTextClip;
		ResultManager.OnLoadProgressBarClip += SetProgressBarClip;
		ResultManager.OnLoadFinalClip += SetFinalClip;
	}
	
	void OnDisable(){
		ResultManager.OnLoadStartClip -= SetStartClip;
		ResultManager.OnLoadTextClip -= SetTextClip;
		ResultManager.OnLoadProgressBarClip -= SetProgressBarClip;
		ResultManager.OnLoadFinalClip -= SetFinalClip;
	}

	void SetStartClip() {
		fx.clip = startClip;
		fx.Play();
	}

	void SetTextClip() {
		fx.clip = textClip;
		fx.Play();
	}

	void SetProgressBarClip() {
		fx.clip = progressBarClip;
		fx.Play();
	}

	void SetFinalClip(float percent) {

		if (percent < 25)
			fx.clip = finalBadClip;
		else if ((percent >= 25) && (percent < 50))
			fx.clip = finalRegularClip;
		else if ((percent >= 50) && (percent < 75))
			fx.clip = finalGoodClip;
		else if (percent >= 75)
			fx.clip = finalPerfectClip;

		fx.Play();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
