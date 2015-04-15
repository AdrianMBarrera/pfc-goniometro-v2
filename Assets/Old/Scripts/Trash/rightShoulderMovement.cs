using UnityEngine;
using System.Collections;

public class rightShoulderMovement : MonoBehaviour {
	
	Material material;
	Color color = Color.white;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKey(KeyCode.LeftShift))
//			seleccionarArticulacion();
//		else
//			rotarArticulacion();

//		ResetearColor();
	}

//	void seleccionarArticulacion () {
//		if (Input.GetMouseButton(0)) {
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//			
//			if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals("HombroD")))
//				renderer.material.color = color = Color.green;
//		}
//		else if (Input.GetMouseButton(1)) {
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//			
//			if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals("HombroD")))
//				renderer.material.color = color = Color.white;
//		}
//	}

	void rotarArticulacion() {
		if (Input.GetMouseButton(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals("HombroD"))) {
				renderer.material.color = Color.red;
				rotarHombro(0);
			}
		}
		else if (Input.GetMouseButton(1)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals("HombroD"))) {
				renderer.material.color = Color.red;
				rotarHombro(1);
			}
		}
	}
	
	void rotarHombro(int raton) {
		Quaternion newRotation = Quaternion.Euler(0,0,0);
		if (raton == 0) {
			if (Camera.main.name.Equals("CamaraFrontal"))
				newRotation = Quaternion.Euler(0,0,1) * transform.rotation;
			else if (Camera.main.name.Equals("CamaraDerecha"))
				newRotation = Quaternion.Euler(1,0,0) * transform.rotation;
			else if (Camera.main.name.Equals("CamaraIzquierda"))
				newRotation = Quaternion.Euler(-1,0,0) * transform.rotation;
			else if (Camera.main.name.Equals("CamaraCenital"))
				newRotation = Quaternion.Euler(0,1,0) * transform.rotation;
		}
		
		if (raton == 1) {
			if (Camera.main.name.Equals("CamaraFrontal"))
				newRotation = Quaternion.Euler(0,0,-1) * transform.rotation;
			else if (Camera.main.name.Equals("CamaraDerecha"))
				newRotation = Quaternion.Euler(-1,0,0) * transform.rotation;
			else if (Camera.main.name.Equals("CamaraIzquierda"))
				newRotation = Quaternion.Euler(1,0,0) * transform.rotation;
			else if (Camera.main.name.Equals("CamaraCenital"))
				newRotation = Quaternion.Euler(0,-1,0) * transform.rotation;
		}
		
		transform.rotation = newRotation;
	}
	
	void ResetearColor() {
		if ((Input.GetMouseButtonUp(0)) || (Input.GetMouseButtonUp(1)))
			renderer.material.color = color;
	}
}

