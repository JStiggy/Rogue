using System.Collections.Generic;
using System.Xml.Serialization;

//Contains all data for skill stats
[XmlRoot("SkillData")]
public class SkillData
{
    [XmlArray("Skills")]
    [XmlArrayItem("Skill")]
    public List<Skill> Skills = new List<Skill>();
}
