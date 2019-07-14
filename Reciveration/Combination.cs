using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combination : Reciveration
{
    public override bool CanHandle
    {
        get
        {
            foreach (Bonduration bond in gameObject.GetComponentsInChildren<Bonduration>())
            {
                if (!bond.CanHandle)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public override void Actions()
    {

    }

    public override void Collision(string eid, Collision2D target, CollisionType type)
    {

    }

    public override void Init()
    {
        foreach(Modurnation modurnation in transform.GetComponentsInChildren<Modurnation>())
        {
            modurnation.parentReciveration = this;
            if (modurnation.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Destroy(modurnation.gameObject.GetComponent<Rigidbody2D>());
            }
        }
        Rigidbody2D rigid = gameObject.AddComponent<Rigidbody2D>();
        rigid.gravityScale = 0.0f;
        rigid.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void OnDynamicStateChange(DynamicState cState)
    {

    }

    public override void Trigger(string eid, Collider2D target, CollisionType type)
    {

    }

    public override void UpdateState()
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
