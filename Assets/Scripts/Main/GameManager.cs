using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameLibrary gameLibrary;

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
}