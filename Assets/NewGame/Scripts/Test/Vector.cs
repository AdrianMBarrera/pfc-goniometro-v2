//Clase que define un vector en un espacio tridimensional y como trabajar con él

public class Vector {
	
	private float X;
	private float Y;
	private float Z;
	public string name;
	
	public Vector() {
		X = 0; Y = 0; Z = 0;
		name = "";
	}
	
	public Vector (Vector vec) {
		X = vec.GetX(); Y = vec.GetY(); Z = vec.GetZ();
		name = vec.GetName();
	}
	
	public void SetX(float x) {X = x;}
	public void SetY(float y) {Y = y;}
	public void SetZ(float z) {Z = z;}
	public void SetName(string n) {name = n;}
	
	public float GetX() {return X;}
	public float GetY() {return Y;}
	public float GetZ() {return Z;}
	public string GetName() {return name;}
}
