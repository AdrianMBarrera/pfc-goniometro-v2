using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Xml;

public class CreateInstanceButtonsScript : MonoBehaviour {

	public GameObject prefabButton;
	public int sizeButton = 50;
	public string path;
	private GameObject buttonPool;

	private RectTransform poolTransform; // rectTRansform del pool de los botones

	// Use this for initialization
	void Start () {
		buttonPool = GameObject.Find("ButtonPoolPlanInst");
		poolTransform = buttonPool.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateButton(string n){
		
		GameObject button = Instantiate(prefabButton, prefabButton.transform.position, Quaternion.identity) as GameObject;
		//		prefabButton.transform.parent = this.transform;
		button.name = n;
		button.transform.SetParent(buttonPool.transform, false);
		//button.transform.parent = buttonPool.transform;

		RectTransform rectTransform = button.GetComponent<RectTransform>();

		//		rectTransform.anchoredPosition = new Vector2 (1f, y);
		//		rectTransform.sizeDelta = new Vector2 (-45,40);

		Text t = button.GetComponentInChildren<Text>();
		

		t.text = n.Substring(0,n.Length-4);

		
		poolTransform.sizeDelta = new Vector2(poolTransform.sizeDelta.x, poolTransform.sizeDelta.y + sizeButton);
		poolTransform.anchoredPosition = new Vector2 (0, -(poolTransform.rect.height + sizeButton));

		Destroy(button.transform.FindChild("Toggle").gameObject);
	}

	public void LoadInstances(){

		for (int i = 0; i < buttonPool.transform.childCount; i++) {
			Destroy(buttonPool.transform.GetChild(i).gameObject);
		}
		poolTransform.sizeDelta = new Vector2(poolTransform.sizeDelta.x, -192.6f);
		poolTransform.anchoredPosition = new Vector2 (0, 0);

		XmlDocument xDoc = new XmlDocument();
		xDoc.Load("./Plans/" + name);
		XmlNodeList plan = xDoc.GetElementsByTagName("PLAN");	
		XmlNodeList listInstances = ((XmlElement)plan[0]).GetElementsByTagName("scheduleList");
		XmlNodeList nodeInstance = xDoc.GetElementsByTagName("nameInstance");

		for (int i = 0; i < nodeInstance.Count; i++) {
			string nameInstance = nodeInstance[i].InnerText;
			CreateButton(nameInstance);
		}

	}
}
