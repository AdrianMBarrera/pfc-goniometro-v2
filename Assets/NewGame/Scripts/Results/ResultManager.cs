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

	public Animator _anim;

	public Button lessResults;
	public Button moreResults;

	private float percent = 100f;

	public Text moreInfoText;


	
	void Start () {
		pb = GameObject.Find("ProgressBarLabelFollow").GetComponent<ProgressBar.ProgressBarBehaviour>();
		StartCoroutine(ShowResults());
		lessResults.GetComponent<Image>().enabled = false;

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
			moreInfoText.GetComponent<RectTransform>().sizeDelta = new Vector2 (moreInfoText.GetComponent<RectTransform>().sizeDelta.x,
			                                                                    moreInfoText.GetComponent<RectTransform>().sizeDelta.y+190f);
			success += InfoPlayer.alExercise[i].Success;
			fails += InfoPlayer.alExercise[i].Fail;
			reps += InfoPlayer.alExercise[i].Success + InfoPlayer.alExercise[i].Fail;
			time += InfoPlayer.alExercise[i].Duration;
			int m = (int)(InfoPlayer.alExercise[i].Duration/60f);
			int s = (int)(InfoPlayer.alExercise[i].Duration%60f);
			moreInfoText.text += "\n\n\n<size=25>Exercise: " + InfoPlayer.alExercise[i].FileName + "</size>" +
				"\n\n\tTime: " + string.Format("{0:00}:{1:00}", m ,s) +
				"\n\n\tRepetitions: " + (InfoPlayer.alExercise[i].Success + InfoPlayer.alExercise[i].Fail).ToString() +
				"\n\n\tSuccess: " + InfoPlayer.alExercise[i].Success.ToString() +
				"\n\n\tFails: " + InfoPlayer.alExercise[i].Fail.ToString();
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


	public void ResetList() {
		InfoPlayer.alExercise.Clear();
	}

	public void ShowMoreResults () {
		_anim.SetBool("isHidden", true);
		moreResults.GetComponent<Image>().enabled = false;
		lessResults.GetComponent<Image>().enabled = true;
	}

	public void ShowLessResults () {
		_anim.SetBool("isHidden", false);
		moreResults.GetComponent<Image>().enabled = true;
		lessResults.GetComponent<Image>().enabled = false;
	}


}
