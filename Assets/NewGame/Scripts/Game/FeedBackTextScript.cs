using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FeedBackTextScript : MonoBehaviour {

	
	private Animator _readyAnim;
	private Text _t;
	private RectTransform _rt;

	public string good;

	public string bad;

	public string excellent;

	public Gradient grad;

	public Vector2[] v2min;

	public Vector2[] v2max;

	
	void OnEnable(){
		//GameManager.OnLoadGamePhase += ReadyAnimation;
		GameManager.OnInGamePhase += SetOn;
		GameManager.OnDemostrationPhase += SetOff;
		GameManager.OnCalibrationPhase += SetOff;
		GameManager.OnCheckFeedBack += SetFeedBack;
	}
	
	
	void OnDisable(){
		//GameManager.OnLoadGamePhase += ReadyAnimation;
		GameManager.OnInGamePhase -= SetOn;
		GameManager.OnDemostrationPhase -= SetOff;
		GameManager.OnCalibrationPhase -= SetOff;
		GameManager.OnCheckFeedBack -= SetFeedBack;
	}
	
	
	// Use this for initialization
	void Start () {
		_readyAnim = GetComponent<Animator>();
		
		_t = GetComponent<Text>();

		_rt = GetComponent<RectTransform>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	void SetFeedBack(){

		int rand = Random.Range(0,v2max.Length);

		Debug.Log("rand" + rand);
		if (GameManager.instance.maxAngle >= 90){

			int randText = Random.Range(0, 2);

			if (randText >0 ){

				excellent = "PERFECT!";
			}else{
				excellent = "EXCELLENT!";

			}


			AssignValues(excellent, 
			             v2min[rand],
			             v2max[rand]);

		}else if  (GameManager.instance.maxAngle >= 65){

				AssignValues(good, 
			             v2min[rand],
			             v2max[rand]);
				
			}else{
				AssignValues(bad, 
			             v2min[rand],
			             v2max[rand]);
				
			}


		_t.color = grad.Evaluate(GameManager.instance.maxAngle/100);

		_t.enabled = true;
		_readyAnim.Play("Idle");
		_readyAnim.SetBool("isBig", true);
	}


	void AssignValues (string t , Vector2 min , Vector2 max){



		_t.text = t;

		_rt.anchorMin = min;
		_rt.anchorMax = max;

		_rt.sizeDelta = new Vector2 (0,0);
		_rt.anchoredPosition = new Vector2 (0,0);
	}


	void SetOn(){
		_t.enabled = true;

	}
	
	void SetOff(){
		
		_t.enabled = false;
	}

}
