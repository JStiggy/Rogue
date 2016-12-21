using System.Xml;
using System.Xml.Serialization;

public class Monster { 
    [XmlAttribute("monsterName")]

    public string monsterName;
    public int health;
    public int mana;

    [XmlArrayItem("stats")]
    public int[] stats;

    [XmlArrayItem("resistances")]
    public float[] resistances;

    [XmlArrayItem("skills")]
    public int[] skills;

    [XmlArrayItem("passives")]
    public int[] passives;
}