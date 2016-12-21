using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("MonsterData")]
public class MonsterData
{
    [XmlArray("Monsters")]
    [XmlArrayItem("Monster")]
    public List<Monster> Monsters = new List<Monster>();
}