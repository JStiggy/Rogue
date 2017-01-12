using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

//Contains all the stat values for a single monster, Values are described in MonsterData.xml
public class Monster { 
    [XmlAttribute("monsterName")]

    public string monsterName;
    public int health;
    public int mana;

    [XmlArrayItem("stats")]
    public int[] stats;

    [XmlArrayItem("resistances")]
    public float[] resistances;

    [XmlArrayItem("statusResistances")]
    public float[] statusResistances;

    [XmlArrayItem("skills")]
    public int[] skills;

    [XmlArrayItem("passives")]
    public int[] passives;

    public Monster Clone()
    {
        Monster tmp = (Monster)(this.MemberwiseClone());
        tmp.stats = (int[])stats.Clone();
        tmp.resistances = (float[])resistances.Clone();
        tmp.statusResistances = (float[])statusResistances.Clone();
        return tmp;
    }
}