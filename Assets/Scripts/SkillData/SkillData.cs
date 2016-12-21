using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("SkillData")]
public class SkillData
{
    [XmlArray("Skills")]
    [XmlArrayItem("Skill")]
    public List<Skill> Skills = new List<Skill>();
}
