using System.Xml;

public class Resultados {
	
	private float tick;//tiempo transcurrido
	private float limit;//intervalor para guardar datos
	private double maxLocal; //maximo local
	private double minLocal; //minimo local
	private double maxGlobal;
	private double minGlobal;
	private double repeticiones; //numero de repeticiones
	
	public Resultados() {
		repeticiones = 0;
		tick = 0; limit = 0.1f;
		maxLocal = 0; maxGlobal = 0;
		minLocal = 180;	minGlobal = 180;
	}
	
	public void SetTick(float x) {tick = x;}
	public void SetLimit(float x) {limit = x;}
	public void SetMaxLocal(double x) {maxLocal = x;}
	public void SetMaxGlobal(double x) {maxGlobal = x;}
	public void SetMinLocal(double x) {minLocal = x;}
	public void SetMinGlobal(double x) {minGlobal = x;}
	public void SetRepeticiones(double x) {repeticiones = x;}
	
	public float GetTick() {return tick;}
	public float GetLimit() {return limit;}
	public double GetMaxLocal() {return maxLocal;}
	public double GetMaxGlobal() {return maxGlobal;}
	public double GetMinLocal() {return minLocal;}
	public double GetMinGlobal() {return minGlobal;}
	public double GetRepeticiones() {return repeticiones;}
	
	
	//Escribe en un fichero los datos obtenidos tras la realizacion del ejercicio
	public void Guardar(string name, double minimo, double maximo, double angulo, Vector Plano) {
		XmlTextWriter writerXml;
		bool found = false;
		
		try {
			byte[] endtag = System.Text.Encoding.UTF8.GetBytes("");
			
			if(!System.IO.File.Exists("./resultados2.xml")) {
				writerXml = new XmlTextWriter ("./resultados2.xml",System.Text.Encoding.UTF8);
				//writerXml.Formatting = Formatting.Indented;
				writerXml.WriteStartDocument();
				writerXml.WriteStartElement("Resultados hueso " + name);
				//				writerXml.Close();
			}
			else {
				found = true;
				System.IO.FileStream fs = System.IO.File.OpenWrite("./resultados2.xml");
				fs.Seek(-endtag.Length, System.IO.SeekOrigin.End);
				writerXml = new XmlTextWriter(fs, System.Text.Encoding.UTF8);
				writerXml.WriteStartElement("Resultados hueso " + name);
			}
			
			writerXml.Indentation = 4;
			writerXml.IndentChar = System.Convert.ToChar(" ");
			writerXml.Formatting = Formatting.Indented;
			
			writerXml.WriteStartElement("Datos");// START CHILD 
			writerXml.WriteAttributeString("Fecha", System.DateTime.Now.ToLongDateString() + 
			                               " " + System.DateTime.Now.ToShortTimeString());
			writerXml.WriteElementString("Minimo", minimo.ToString());
			writerXml.WriteElementString("Maximo", maximo.ToString());
			writerXml.WriteElementString("Angulo", angulo.ToString());
			string plane = Plano.GetX()+","+Plano.GetY()+","+Plano.GetZ(); 
			writerXml.WriteElementString("Plano", plane);
			writerXml.WriteEndElement();//END CHILD

			if (!found) {
				writerXml.WriteEndElement();//END PARENT
				writerXml.WriteEndDocument();
			}
			else {
				writerXml.Flush();
				writerXml.BaseStream.Write(endtag, 0, endtag.Length);
			}
			
			if (writerXml != null)	writerXml.Flush();
			
			if (writerXml.WriteState != System.Xml.WriteState.Closed) writerXml.Close();
		}
		catch {
			throw new System.IO.FileNotFoundException("File ./resultados2.xml not found");
		}
	}
}
