using System.Collections.Generic;
using System.Xml.Serialization;

//Contains all data for skill stats
[XmlRoot("PassiveData")]
public class PassiveData
{
    [XmlArray("Passives")]
    [XmlArrayItem("Passive")]
    public List<Passive> Passives = new List<Passive>();
}
