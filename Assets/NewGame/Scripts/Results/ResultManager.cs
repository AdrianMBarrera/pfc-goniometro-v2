using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {

	// Use this for initialization

	private ProgressBar.ProgressBarBehaviour pb;
	public Gradient grad;
	public Image filler;

	public Text TotalTime;

	public Text TotalReps;

	public Text TotalSuccess;

	public Text TotalFails;

	public Text FeedbackText;

	public Sprite successImage;

	public Sprite FailImage;

	private bool locker = false;

	private float percent = 100f;


	
	void Start () {
		pb = GameObject.Find("ProgressBarLabelFollow").GetComponent<ProgressBar.ProgressBarBehaviour>();
		StartCoroutine(ShowResults());
	}


	
	// Update is called once per frame
	void Update () {
//		Debug.Log("dfkladfaldkja " + filler.GetComponentInC<Text>().text);
		int p = System.Convert.ToInt16(filler.GetComponentInChildren<Text>().text.Replace("%",""));
		if ((percent == p) && (!FeedbackText.gameObject.GetComponent<Animator>().enabled)) {
			if (percent < 25)
				FeedbackText.text = "Bad!";
			else if ((percent >= 25) && (percent < 50))
				FeedbackText.text = "Regular!";
			else if ((percent >= 50) && (percent < 75))
				FeedbackText.text = "Good!";
			else if (percent >= 75)
				FeedbackText.text = "Perfect!";
			FeedbackText.gameObject.GetComponent<Animator>().enabled = true;
		}
	}



	IEnumerator ShowResults(){
		int success = 0, fails = 0, reps = 0;
		float time = 0f;

		for (int i = 0; i < InfoPlayer.alExercise.Count; i++) {
			success += InfoPlayer.alExercise[i].Success;
			fails += InfoPlayer.alExercise[i].Fail;
			reps += InfoPlayer.alExercise[i].Success + InfoPlayer.alExercise[i].Fail;
			time += InfoPlayer.alExercise[i].Duration;
		}

		int minutes = (int)(time/60f);
		int seconds = (int)(time%60f);

		yield return new WaitForSeconds(1f);
		TotalTime.text = string.Format("{0:00}:{1:00}", minutes ,seconds);
		TotalTime.gameObject.GetComponent<Animator>().enabled = true;
//		TotalTime.gameObject.GetComponent<Animator>().SetBool("isBig", true);

		yield return new WaitForSeconds(2f);
		TotalReps.text = reps.ToString();
		TotalReps.gameObject.GetComponent<Animator>().enabled = true;

		yield return new WaitForSeconds(2f);
		TotalSuccess.text = success.ToString();
		TotalSuccess.gameObject.GetComponent<Animator>().enabled = true;

		yield return new WaitForSeconds(2f);
		TotalFails.text = fails.ToString();
		TotalFails.gameObject.GetComponent<Animator>().enabled = true;

		yield return new WaitForSeconds(2f);
//		pb.SetFillerSizeAsPercentage(100);

		switch (InfoPlayer.gameMode) {

			case(InfoPlayer.gameModes.Open):
				if (reps > 0)
					percent = (success * 100) / reps;
				pb.SetFillerSizeAsPercentage(percent);
				//filler.color = grad.Evaluate(percent/100);
				break;
				
			case (InfoPlayer.gameModes.Custom) : break;
				
				
			case (InfoPlayer.gameModes.Preset) : break;
			
		}

		yield return new WaitForSeconds(2f);



	}




	IEnumerator NextResult(){

		locker = false;
		yield return new WaitForSeconds(2f);
		locker = true;

	}


	public void ResetList() {
		InfoPlayer.alExercise.Clear();
	}



}
