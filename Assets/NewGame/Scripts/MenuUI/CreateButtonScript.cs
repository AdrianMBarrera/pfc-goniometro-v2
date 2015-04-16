using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class CreateButtonScript : MonoBehaviour {
	public GameObject prefabButton;
	public string path;
	public int sizeButton = 50;

	private RectTransform poolTransform; // rectTRansform del pool de los botones
	// Use this for initialization
	void Start () {
		poolTransform = GameObject.Find("buttonPool").GetComponent<RectTransform>();

		ReadXml(path);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("fdafdasfasdf" + rectTransform.rect);
	}

	public void CreateButton(float y, string name){

		prefabButton = Instantiate(prefabButton,prefabButton.transform.position, Quaternion.identity) as GameObject;
//		prefabButton.transform.parent = this.transform;
		prefabButton.transform.SetParent(this.transform, false);

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

	public void ReadXml(string path){
		float y = -20f;
		DirectoryInfo di = new DirectoryInfo(path);
		FileInfo[] files = di.GetFiles();

		foreach (FileInfo fi in files)
		{
			if (fi.Extension.Contains("xml")) {
				CreateButton(y, fi.Name);
				y -=40;
			}
		}

		//poolTransform.sizeDelta = new Vector2(poolTransform.sizeDelta.x, poolTransform.sizeDelta.y +40);
		//poolTransform.anchoredPosition = new Vector2 (0, poolTransform.anchoredPosition.y + 40);

	}
}
