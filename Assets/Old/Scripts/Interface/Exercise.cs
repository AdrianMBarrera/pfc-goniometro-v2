using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRootAttribute("EXERCISE")]
public class Exercise {

	[XmlAttribute("initialId")]
	public int initialId;

	[XmlAttribute("finalId")]
	public int finalId;

	[XmlIgnoreAttribute]
	public string initialArt = "";

	[XmlIgnoreAttribute]
	public string finalArt = "";

	public Angle ang;
	
	public Axis axis;
		
	public Ini ini;

	public RotIni rotIni;

	public RotEnd rotEnd;
	
	public Reference reference;

	public List<Restriction> Restrictions;

	public Exercise() {
		ang = new Angle();
		axis = new Axis();
		ini = new Ini();
		rotIni = new RotIni();
		rotEnd = new RotEnd();
		reference = new Reference();
		Restrictions = new List<Restriction>();
	}
	
	public void Save(string path) {
		XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
		ns.Add("","");

		XmlSerializer serializer = new XmlSerializer(typeof(Exercise));
		Stream stream = new FileStream(path, FileMode.Create);
		serializer.Serialize(stream, this, ns);
		stream.Close();
	}
}

public class Reference {
	
	[XmlAttribute("id")]
	public int id;

	[XmlIgnoreAttribute]
	public string nameId = "";
	
	[XmlAttribute("x")]
	public string x = "";
	
	[XmlAttribute("y")]
	public string y = "";
	
	[XmlAttribute("z")]
	public string z = "";

	public Reference() {}
}

public class Angle {
	[XmlAttribute("min")]
	public string Min = "0";
	
	[XmlAttribute("max")]
	public string Max = "0";

	public Angle() {}
}

public class Axis {
	[XmlAttribute("x")]
	public string X = "";
	
	[XmlAttribute("y")]
	public string Y = "";
	
	[XmlAttribute("z")]
	public string Z = "";

	public Axis() {}
}

public class Ini {
	[XmlAttribute("x")]
	public string x = "";
	
	[XmlAttribute("y")]
	public string y = "";
	
	[XmlAttribute("z")]
	public string z = "";

	public Ini() {}
}

public class RotIni {
	[XmlAttribute("x")]
	public string x = "";
	
	[XmlAttribute("y")]
	public string y = "";
	
	[XmlAttribute("z")]
	public string z = "";
	
	public RotIni() {}
}

public class RotEnd {
	[XmlAttribute("x")]
	public string x = "";
	
	[XmlAttribute("y")]
	public string y = "";
	
	[XmlAttribute("z")]
	public string z = "";
	
	public RotEnd() {}
}

public class Restriction {
	[XmlElement("initialId")]
	public int initialId;

	[XmlElement("finalId")] 
	public int finalId;

	[XmlIgnoreAttribute]
	public string initialArt = "";	

	[XmlIgnoreAttribute]	
	public string finalArt = "";

	public int x, y, z;

	public int rotX, rotY, rotZ;
	
	public int grade = 0;

	public Restriction() {}
}
