using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reciveration : MonoBehaviour ,CollisionReciver {
    public void Collision(string eid, Collision target, CollisionType type)
    {

    }

    public void Trigger(string eid, Collider target, CollisionType type)
    {

    }

    public virtual void Init()
    {

    }

    // Use this for initialization
    void Start () {
        Init();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
