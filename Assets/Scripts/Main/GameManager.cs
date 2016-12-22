using UnityEngine;
using System.IO;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameLibrary gameLibrary;
    public BoardManager board;
    public SaveData playerData;

    //Create a singleton to contain all Gamedata
    private static GameManager manager = null;
    public static GameManager Manager
    {
        get { return manager; }
    }

    //If applicable, set up a singleton
    void Awake()
    {
        if (manager != this && manager != null)
        {
            Destroy(this.gameObject);
        }

        manager = this;
        DontDestroyOnLoad(this.gameObject);
        gameLibrary = gameObject.AddComponent<GameLibrary>();
        board = gameObject.AddComponent<BoardManager>();
        playerData = new SaveData();

        if (!File.Exists(Application.dataPath + "\\Data\\player.dat"))
            playerData.Save();
        else
            playerData.Load();
    }

    //Create a monster based on the index in the XML file
    public GameObject CreateMonster(int index)
    {
        if(index >= gameLibrary.monsterData.Monsters.Count)
        {
            Debug.LogError("Index out of range, cannot create monster of index " + index);
        }
        GameObject monster = new GameObject();
        Enemy traits = monster.AddComponent<Enemy>();
        traits.Create(gameLibrary.monsterData.Monsters[index]);
        return monster;
    }

    //Index is the save value index, not the monsyter index 0,1,2,3
    public GameObject CreatePlayer(int index)
    {
        print(index);
        index = playerData.unitID[index];
        if (index < 0) return null;
        GameObject ally = new GameObject();
        Enemy traits = ally.AddComponent<Enemy>();
        traits.Create(gameLibrary.monsterData.Monsters[index]);
        return ally;
    }
}