using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class GameLibrary : MonoBehaviour
{

    public MonsterData monsterData;
    public SkillData skillData;
    public ItemData itemData;

    //Compile all data (stats, skills, etc)
    void Start()
    {
        //Compile all monster info from the XML file
        XmlSerializer serializer = new XmlSerializer(typeof(MonsterData));
        FileStream stream = new FileStream(Application.dataPath + "\\Data\\MonsterData.xml", FileMode.Open);
        monsterData = serializer.Deserialize(stream) as MonsterData;
        stream.Close();

        //Compile all skill info from the XML file
        serializer = new XmlSerializer(typeof(SkillData));
        stream = new FileStream(Application.dataPath + "\\Data\\SkillData.xml", FileMode.Open);
        skillData = serializer.Deserialize(stream) as SkillData;
        stream.Close();

        serializer = new XmlSerializer(typeof(ItemData));
        stream = new FileStream(Application.dataPath + "\\Data\\ItemData.xml", FileMode.Open);
        itemData = serializer.Deserialize(stream) as ItemData;
        stream.Close();
    }
}
