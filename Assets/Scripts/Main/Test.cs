using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("S");
	    foreach(Collider2D c in Physics2D.OverlapCircleAll(transform.position,4.90f))
        {
            print(c.gameObject.name);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
