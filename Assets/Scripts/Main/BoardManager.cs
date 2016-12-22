using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    Queue<Unit> unitQueue = null;

    void Start()
    {
        unitQueue = new Queue<Unit>();
        for(int i = 0; i < 4; ++i)
        {
            GameObject p = GameManager.Manager.CreatePlayer(i);
            if (p == null) continue;
            unitQueue.Enqueue( p.GetComponent<Unit>() );
        }    
    }

    public void EndTurn()
    {
        Unit t = unitQueue.Dequeue();
        while(t.currentHealth <= 0)
        {
            Destroy(t.gameObject);
            t = unitQueue.Dequeue();
        }
        t.StartCoroutine("StartTurn");
        
    }
}
