using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameLibrary gameLibrary;
    public BoardManager board;
    public SaveData playerData;
    public List<Item> inventory;
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
        LoadInventory();
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

    //Index is the save value index, not the monster index 0,1,2,3
    public GameObject CreatePlayer(int index)
    {
        index = playerData.unitID[index];
        if (index < 0) return null;
        GameObject ally = new GameObject();
        Ally traits = ally.AddComponent<Ally>();
        traits.Create(gameLibrary.monsterData.Monsters[index]);
        return ally;
    }

    public void LoadInventory()
    {
        inventory = new List<Item>();
        for (int i = 0; i< playerData.inventory.Length; ++i)
        {
            if(playerData.inventory[i] == -1)
            {
                break;
            }
            else
            {
                Item item = gameLibrary.itemData.Items[playerData.inventory[i]].clone();
                item.remainingUses = playerData.inventoryStackSize[i];
                inventory.Add(item);
            }
        }
    }

    public void SaveInventory()
    {
        for(int i = 0; i < 5; ++i)
        {
            if(i < inventory.Count)
            {
                playerData.inventory[i] = inventory[i].id;
                playerData.inventoryStackSize[i] = inventory[i].remainingUses;
            }
            else
            {
                playerData.inventory[i] = -1;
            }
        }
    }

    public void DecrementInventory(int index)
    {
        print("Decrementing: " + index);
        if (index == -1) return;
        inventory[index].remainingUses--;
        if(inventory[index].remainingUses <= 0)
        {
            inventory.RemoveAt(index);
        }
    }
}