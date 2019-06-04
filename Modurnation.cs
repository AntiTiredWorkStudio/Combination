using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modurnation : Reciveration
{
    Rigidbody2D targetRigid;
    public Transform Body;
    public override void Collision(string eid, Collision2D target, CollisionType type)
    {
        Debug.Log(target.gameObject.name+","+eid);
    }

    public override void Init()
    {
        targetRigid = GetComponent<Rigidbody2D>();
        Body = transform.Find("Body");
        if (Body != null)
        {
            foreach (Transform target in Body.GetComponentsInChildren<Transform>())
            {
                if (target.gameObject.GetComponent<Collider2D>())
                {
                //    Debug.LogWarning(target.gameObject.name + "not have collider2D");
                }
            }
        }
        SetDynamicState(DynamicState.Dynamic);
        SetDynamicState(DynamicState.Stable);
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
}
