using UnityEngine;
using System.Collections;

public class RotateSphere : MonoBehaviour {
	
	public float Speed = 5f;
	public string Art = "";
	public int Step = 0;
	
	bool selectBlue	 = false;
	bool selectGreen = false;
	bool selectRed	 = false;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update() {
		if (Art.Equals("")){
			transform.position = new Vector3 (-7f,-10f,0);
		}
		
		
		if ((!Art.Equals("")) && (Step < 2)) {
			transform.position = GameObject.Find(Art).transform.position;
			transform.rotation = GameObject.Find(Art).transform.rotation;
			
			if (Input.GetMouseButtonUp(0)){
				selectBlue = false;
				selectRed = false;
				selectGreen = false;
			}

			if (Input.GetMouseButton(0)) {
				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				
				if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals("BlueCircle")) &&
				    (!selectGreen) && (!selectRed))
				{
					selectBlue = true;
					
				}
				if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals("RedCircle")) &&
				    (!selectBlue) && (!selectGreen))
				{
					selectRed = true;
					
				}
				if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals("GreenCircle")) &&
				    (!selectBlue) && (!selectRed))
				{
					selectGreen = true;
					
				}
				
				if ((selectBlue) && Input.GetMouseButton(0)){
					transform.Rotate(Vector3.right * Input.GetAxis("Mouse Y") * Speed);
					GameObject.Find(Art).transform.rotation = transform.rotation;
				}
				if ((selectRed) && Input.GetMouseButton(0)){
					transform.Rotate(Vector3.down * Input.GetAxis("Mouse X") * Speed);
					GameObject.Find(Art).transform.rotation = transform.rotation;
				}
				if ((selectGreen) && Input.GetMouseButton(0)){
					transform.Rotate(Vector3.back * Input.GetAxis("Mouse Y") * Speed);
					GameObject.Find(Art).transform.rotation = transform.rotation;
				}
				
				
			}
		}
	}
}