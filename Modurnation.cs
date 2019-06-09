using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modurnation : Reciveration
{
    Rigidbody2D targetRigid;
    public Transform Body;
    CollisionRecord SelfCollisionRecord;

    public override bool CanHandle
    {
        get
        {
            foreach(Bonduration bond in gameObject.GetComponentsInChildren<Bonduration>())
            {
                if (!bond.CanHandle)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public override void Collision(string eid, Collision2D target, CollisionType type)
    {
        Debug.Log(target.gameObject.name+","+eid);
    }

    public override void Init()
    {
        SelfCollisionRecord = gameObject.AddComponent<CollisionRecord>();
        SelfCollisionRecord.enabled = false;
        SelfCollisionRecord.eventid = "module";

        targetRigid = GetComponent<Rigidbody2D>();

        Body = transform.Find("Body");
        if (Body != null)
        {
            foreach (Transform target in Body.GetComponentsInChildren<Transform>())
            {
                if (target.gameObject.GetComponent<Collider2D>())
                {

                }
            }
        }
        SetDynamicState(DynamicState.Dynamic);
        SetDynamicState(DynamicState.Stable);

        SelfCollisionRecord.enabled = true;
    }

    public override void OnDynamicStateChange(DynamicState cState)
    {
        if(targetRigid != null)
        {
            if (cState == DynamicState.Stable)
            {
                targetRigid.constraints = RigidbodyConstraints2D.FreezeAll;
               // targetRigid.bodyType = RigidbodyType2D.Static;
             //   targetRigid.Sleep();
            }
            else
            {
                targetRigid.constraints = RigidbodyConstraints2D.None;
               // targetRigid.bodyType = RigidbodyType2D.Dynamic;
                //targetRigid.WakeUp();
            }
        }
    }

    public override void Trigger(string eid, Collider2D target, CollisionType type)
    {

    }

    public override void UpdateState()
    {
        foreach (Reciveration recive in gameObject.GetComponentsInChildren<Reciveration>(true))
        {
            if (recive == this)
            {
                continue;
            }
            recive.UpdateState();
        }
    }
}
