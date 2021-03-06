﻿using System.Xml;
using System.Xml.Serialization;

//Container for all data for a skill, Values are described in SkillData.xml
public class Skill {
    public int type;
    public string name;
    public string description;
    public int element;
    public int targetType;
    public int attackType;
    public int defenseType;
    public int flatPower;
    public float modifierPower;
    public int cost;
    [XmlArrayItem("status")]
    public int[] status;
    public int accModifier;
    public int critModifier;
    public float animationTime;
    public int splashRange;
    public int targetRange = 0;
    public Skill Clone()
    {
        Skill temp = (Skill)this.MemberwiseClone();
        temp.status = (int[])status.Clone();
        return temp;
    }
}
