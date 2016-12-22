using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class GameLibrary : MonoBehaviour
{

    public MonsterData monsterData;
    public SkillData skillData;

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

        //Purely testing here
        GameObject g = GameManager.Manager.CreateMonster(0);
        g.transform.position = new Vector3(0, -5, 0);
        GameObject g3 = GameManager.Manager.CreateMonster(0);
        g3.transform.position = new Vector3(1, -5, 0);
        GameObject g1 = GameManager.Manager.CreateMonster(0);
        g1.transform.position = new Vector3(2, -5, 0);
        GameObject g4 = GameManager.Manager.CreateMonster(0);
        g4.transform.position = new Vector3(0, -6, 0);
        GameObject g5 = GameManager.Manager.CreateMonster(0);
        g5.transform.position = new Vector3(0, -7, 0);
        GameObject g6 = GameManager.Manager.CreateMonster(0);
        g6.transform.position = new Vector3(1, -6, 0);
        GameObject g7 = GameManager.Manager.CreateMonster(0);
        g7.transform.position = new Vector3(1, -7, 0);
        GameObject g8 = GameManager.Manager.CreateMonster(0);
        g8.transform.position = new Vector3(2, -6, 0);
        GameObject g9 = GameManager.Manager.CreateMonster(0);
        g9.transform.position = new Vector3(2, -7, 0);

        GameObject g2 = GameManager.Manager.CreateMonster(1);
        g2.transform.position = new Vector3(0, -2, 0);
        g2.GetComponent<Enemy>().UseSkill(0);
    }
}
