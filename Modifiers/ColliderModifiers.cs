using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderModifiers : Modifiers
{
    public enum ColliderSize
    {
        NORMAL,LARGE
    }

    public List<Collider2D> targets;
    Dictionary<Collider2D, bool> triggerstateList;

	// Use this for initialization
	public void Init () {
        targets = new List<Collider2D>(GetComponentsInChildren<Collider2D>());
        triggerstateList = new Dictionary<Collider2D, bool>();
        foreach (Collider2D collid in targets)
        {
            triggerstateList.Add(collid, collid.isTrigger);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MagicCollider(bool istrigger)
    {
        if(targets == null)
        {
            Debug.LogWarning("targets is null:"+ istrigger);
        }
        foreach(Collider2D collid in targets)
        {
            if (!triggerstateList[collid])
            {
                collid.isTrigger = istrigger;
            }
        }
    }
}
