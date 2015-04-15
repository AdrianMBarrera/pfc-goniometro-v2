using UnityEngine;
using System.Collections;

public class DeleteRestrictionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Delete() {
		string parentName = gameObject.transform.parent.name;
		GameObject.Find("RestrictionsInterface").SendMessage("DeleteRestriction", parentName, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject.transform.parent.gameObject);
	}
}
