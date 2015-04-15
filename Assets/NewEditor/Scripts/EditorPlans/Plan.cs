using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRootAttribute("PLAN")]
public class Plan {

	public List<Schedule> scheduleList;

	public Plan() {
		scheduleList = new List<Schedule>();
	}
	
	public void Save(string path) {
		XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
		ns.Add("","");
		XmlSerializer serializer = new XmlSerializer(typeof(Plan));
		Stream stream = new FileStream(path, FileMode.Create);
		serializer.Serialize(stream, this, ns);
		stream.Close();
	}


}
public class Schedule {
	[XmlElement("")]
	public string nameInstance;
	public Schedule() {}
}


