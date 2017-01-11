using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    public List<Unit> allies = null;
    Queue<Unit> unitQueue = null;
    public BoardMenuManager menu = null;
    public Unit currentUnit = null;

    void Start()
    {
        menu = GameObject.Find("UI").GetComponent<BoardMenuManager>();
        allies = new List<Unit>();
        unitQueue = new Queue<Unit>();
        for(int i = 0; i < 4; ++i)
        {
            GameObject p = GameManager.Manager.CreatePlayer(i);
            if (p == null) continue;
            unitQueue.Enqueue( p.GetComponent<Unit>() );
            allies.Add(p.GetComponent<Unit>());
        }
        GameManager.Manager.CreateMonster(0).transform.position = new Vector3(1, 1, 0);
        GameManager.Manager.CreateItem(1).transform.position = new Vector3(-1, -1, 0);
        EndTurn();

    }

    public void EndTurn()
    {
        Unit t = unitQueue.Dequeue();
        while(t.currentHealth <= 0 && unitQueue.Count > 0)
        {
            Destroy(t.gameObject);
            t = unitQueue.Dequeue();
        }
        if (unitQueue.Count != 0 || t.currentHealth > 0)
        {
            unitQueue.Enqueue(t);
            //print("Start Turn: " + t.name);
            currentUnit = t;
            menu.UpdateHUD();
            t.StartCoroutine("StartTurn");
        }
        else
        {
            //So this case really shouldnt happen during standard gameplay, I ran into it during testing on a blank scene with only the player, targeted myself and died
             print("This case shouldn't really happen, you dismissed all allies, killed all enemies and then targeted yourself with an attack/died from poison. Why?");
        }
    }
}