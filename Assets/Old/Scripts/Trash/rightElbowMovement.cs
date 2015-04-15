using UnityEngine;
using System.Collections;

public class rightElbowMovement : MonoBehaviour {

	Material material;
	Vector3 pos_inicial;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
//		if (Input.GetMouseButton(0)) {
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//			
//			if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals("CodoD"))) {
//				renderer.material.color = Color.red;
//				rotarCodo(0);
//			}
//		}
//		
//		if (Input.GetMouseButton(1)) {
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			RaycastHit hit;
//			if ((Physics.Raycast(ray, out hit)) && (hit.collider.gameObject.name.Equals("CodoD"))) {
//				renderer.material.color = Color.red;
//				rotarCodo(1);
//			}
//		}
//		ResetearColor();
	}

	void rotarCodo(int raton) {
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
			renderer.material.color = Color.white;
	}
}
