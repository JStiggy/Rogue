using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class GameLibrary : MonoBehaviour
{

    public MonsterData monsterData;
    public SkillData skillData;

    // Use this for initialization
    void Start()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(MonsterData));
        FileStream stream = new FileStream(Application.dataPath + "\\Data\\MonsterData.xml", FileMode.Open);
        monsterData = serializer.Deserialize(stream) as MonsterData;
        stream.Close();

        serializer = new XmlSerializer(typeof(SkillData));
        stream = new FileStream(Application.dataPath + "\\Data\\SkillData.xml", FileMode.Open);
        skillData = serializer.Deserialize(stream) as SkillData;
        stream.Close();

        GameObject g2 = GameManager.Manager.CreateMonster(0);
        GameObject g = GameManager.Manager.CreateMonster(0);
        g.transform.position = new Vector3(0,-5, 0);
    }
}
