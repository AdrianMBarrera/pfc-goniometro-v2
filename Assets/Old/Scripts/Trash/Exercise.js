import System.Collections.Generic;
import System.Xml.Serialization;

@XmlRoot("EXERCISE")
public class EXERCISE {

	@XmlAttribute("initialId")
	public var initialId ;
	
	@XmlAttribute("finalId")
	public var finalId ;
	@XmlIgnoreAttribute
	public var initialArt = "";
	@XmlIgnoreAttribute
	public var finalArt = "";
	
	@XmlElement("ANGLE")
	public var ang : Angle;
	
	@XmlElement("AXIS")
	public var eje : Eje;
	

	@XmlElement("INI")
	public var ini : Ini;
	
	@xmlElement("REFERENCE")
	public var reference : Reference;
	
	
	@XmlElement("RESTRICTION")
	public var restrictions : List.<Restriction> = new List.<Restriction>();


	public function Save(path : String) {
		var ns : XmlSerializerNamespaces = new XmlSerializerNamespaces();
		ns.Add("","");
	
 		var serializer : XmlSerializer = new XmlSerializer(EXERCISE);
 		var stream : Stream = new FileStream(path, FileMode.Create);
 		serializer.Serialize(stream, this, ns);
 		stream.Close();
 	}

}

public class Reference {
	
	@XmlAttribute("id")
	public var id ;
	@XmlIgnoreAttribute
	public var nameId = "";
	
	@XmlAttribute("x")
	public var x : String = "";
	
	@XmlAttribute("y")
	public var y : String = "";
	
	@XmlAttribute("z")
	public var z : String = "";
}

public class Angle {
	@XmlAttribute("min")
	public var Min : String = "";
	
	@XmlAttribute("max")
	public var Max : String = "";
}

public class Eje {
	@XmlAttribute("x")
	public var X : String = "";
	
	@XmlAttribute("y")
	public var Y : String = "";
	
	@XmlAttribute("z")
	public var Z : String = "";
}

public class Ini {

	@XmlAttribute("x")
	public var x : String = "";
	
	@XmlAttribute("y")
	public var y : String = "";
	
	@XmlAttribute("z")
	public var z : String = "";
}

public class Restriction {

	@XmlElement("initialId")
	public var initialId : int;
    @XmlElement("finalId") 
	public var finalId : int;
	@XmlIgnoreAttribute
	public var initialArt = "";	
	@XmlIgnoreAttribute	
	public var finalArt = "";
	
	
	
	@XmlElement("x")
	public var x : int;
	@XmlElement("y")
	public var y : int;
	@XmlElement("z")
	public var z : int;

	@XmlElement("grade")
	public var grade = 0;
}