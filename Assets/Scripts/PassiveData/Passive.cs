using System.Xml;
using System.Xml.Serialization;

public class Passive
{
    public string name;
    public string description;
    public int type = 0;
    public int requirement = -1;
    public int requirementParameter = -1;
    //Bonus used for damage multipliers etc
    public float valueBonus = 1;
    //Changes to character core stats 
    [XmlArrayItem("stats")]
    public int[] statBonus = { };
    //Changes to character elemental affinities
    [XmlArrayItem("stats")]
    public int[] resistanceBonus = { };
    //Changes to characters status resistances
    [XmlArrayItem("stats")]
    public int[] statusBonus = { };
}
