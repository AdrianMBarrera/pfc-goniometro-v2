#pragma strict

var speed : float = 5f;
var art : String = "";


function Start () {

}

function Update () {

	if (!art.Equals("")) {
		
		transform.position = GameObject.Find(art).transform.position;
	
		if (Input.GetMouseButton(0)) {
			var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			var hit : RaycastHit;
			
			if ((Physics.Raycast(ray, hit)) && (hit.collider.gameObject.name.Equals("blueCircle"))) {
				transform.Rotate(Vector3.right * Input.GetAxis("Mouse Y")* speed);
				GameObject.Find(art).transform.rotation = transform.rotation;
			}
			else if ((Physics.Raycast(ray, hit)) && (hit.collider.gameObject.name.Equals("greenCircle"))) {
				transform.Rotate(Vector3.back * Input.GetAxis("Mouse Y") * speed);
				GameObject.Find(art).transform.rotation = transform.rotation;
			}
			else if ((Physics.Raycast(ray, hit)) && (hit.collider.gameObject.name.Equals("redCircle"))) {
				transform.Rotate(Vector3.down * Input.GetAxis("Mouse X") * speed);
				GameObject.Find(art).transform.rotation = transform.rotation;
			}
		}
	}
}