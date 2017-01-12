using System.Xml;
using System.Xml.Serialization;

public class Passive
{
    //No passives can alter Max HP or MP based on the current setup, this is intentional HP is bascially Mind and Vitaility in terms of defenses
    public string name;
    public string description;
    public int type = 0;
    public int requirement = -1;
    public int requirementParameter = -1;
    //Bonus used for damage multipliers etc
    public float valueBonus = 1;
    //Changes to character core stats 
    [XmlArrayItem("statBonus")]
    public float[] statBonus = { };
    //Changes to character elemental affinities
    [XmlArrayItem("resistanceBonus")]
    public float[] resistanceBonus = { };
    //Changes to characters status resistances
    [XmlArrayItem("statusBonus")]
    public float[] statusBonus = { };
}
