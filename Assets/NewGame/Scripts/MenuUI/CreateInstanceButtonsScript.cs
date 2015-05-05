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
	public GameObject buttonPool;

	private RectTransform poolTransform; // rectTRansform del pool de los botones

	// Use this for initialization
	void Start () {
		poolTransform = buttonPool.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateButton(string name){
		
		prefabButton = Instantiate(prefabButton,prefabButton.transform.position, Quaternion.identity) as GameObject;
		Debug.Log ("createbutton");
		//		prefabButton.transform.parent = this.transform;
		prefabButton.transform.SetParent(buttonPool.transform, false);
		
		RectTransform rectTransform = prefabButton.GetComponent<RectTransform>();
		//		rectTransform.anchoredPosition = new Vector2 (1f, y);
		//		rectTransform.sizeDelta = new Vector2 (-45,40);
		prefabButton.name = name;
		Text t = prefabButton.GetComponentInChildren<Text>();
		
		
		t.text = name.Substring(0,name.Length-4);
		//		Debug.Log("Tamaño pool " + poolTransform.sizeDelta.y);
		//		Debug.Log("Tamaño pool height " + poolTransform.rect.height);
		//		Debug.Log ("Tamaño height " + rectTransform.rect.height);
		
		poolTransform.sizeDelta = new Vector2(poolTransform.sizeDelta.x, poolTransform.sizeDelta.y + sizeButton);
		poolTransform.anchoredPosition = new Vector2 (0, -(poolTransform.rect.height + sizeButton));
		
		Debug.Log("anchored: " +  poolTransform.anchoredPosition.y);
		Debug.Log("delta: " +  poolTransform.sizeDelta.y);
		
	}

	public void LoadInstances(){
		Debug.Log ("loadinstance");
		
		XmlDocument xDoc = new XmlDocument();
		xDoc.Load("./Plans/" + name);
		XmlNodeList plan = xDoc.GetElementsByTagName("PLAN");	
		XmlNodeList listInstances = ((XmlElement)plan[0]).GetElementsByTagName("ScheduleList");
	
		int i = 0;
		foreach (XmlElement instance in listInstances) {
			Debug.Log ("for");
			XmlNodeList nodeInstance = instance.GetElementsByTagName("nameInstance");
			string nameInstance = nodeInstance[i].InnerText;
			CreateButton(nameInstance);
			i++;
		}

	}
}
