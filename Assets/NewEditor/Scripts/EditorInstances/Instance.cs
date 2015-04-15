using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRootAttribute("INSTANCE")]
public class Instance {
	
	[XmlAttribute("name")]
	public string name;
	
	[XmlAttribute("time")]
	public int time;

	[XmlAttribute("repetitions")]
	public int repetitions;


	public Instance() {
		name = "";
		time = 0;
		repetitions = 0;
	}
	
	public void Save(string path) {
		XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
		ns.Add("","");
		
		XmlSerializer serializer = new XmlSerializer(typeof(Instance));
		Stream stream = new FileStream(path, FileMode.Create);
		serializer.Serialize(stream, this, ns);
		stream.Close();
	}
}

