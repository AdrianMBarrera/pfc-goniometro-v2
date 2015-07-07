using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRootAttribute("RESULTS")]
public class Results {
	
	[XmlAttribute("gameMode")]
	public string gameMode;
	
	public List<ExerciseResult> exercises;
	
	public Results() {
		gameMode = "";
		exercises = new List<ExerciseResult>();
	}
	
	public void Save(string path) {

		XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
		ns.Add("","");
		
		XmlSerializer serializer = new XmlSerializer(typeof(Results));
		Stream stream = new FileStream(path, FileMode.Create);
		serializer.Serialize(stream, this, ns);
		stream.Close();
	}
}

public class ExerciseResult {
	[XmlAttribute("exercise")]
	public string exercise = "";
	
	[XmlAttribute("time")]
	public string time = "";
	
	[XmlAttribute("repetitions")]
	public string repetitions = "";

	[XmlAttribute("success")]
	public string success = "";

	[XmlAttribute("fail")]
	public string fail = "";
	
	public ExerciseResult() {}
}


