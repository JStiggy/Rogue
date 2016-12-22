using System.Collections.Generic;
using System.Xml.Serialization;

//Contains all data for unit stats
[XmlRoot("MonsterData")]
public class MonsterData
{
    [XmlArray("Monsters")]
    [XmlArrayItem("Monster")]
    public List<Monster> Monsters = new List<Monster>();
}