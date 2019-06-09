using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CollisionType
{
    ENTER,EXIT
}
public interface CollisionReciver
{
    void Collision(string eid,Collision2D target, CollisionType type);
    void Trigger(string eid,Collider2D target, CollisionType type);
}
public class CollisionRecord : MonoBehaviour {
    public static T TransforReciver<T>(Collider2D target) where T:Reciveration
    {
        if (target.gameObject.GetComponent<CollisionRecord>().eventObject == null)
        {
            return null;
        }
        return target.gameObject.GetComponent<CollisionRecord>().eventObject as T;
    }

    public static T TransforReciver<T>(Collision2D target) where T : Reciveration
    {
        if(target.collider.gameObject.GetComponent<CollisionRecord>().eventObject == null)
        {
            return null;
        }
        return target.collider.gameObject.GetComponent<CollisionRecord>().eventObject as T;
    }
    
    public string eventid;
    public Reciveration eventObject;
    CollisionReciver targetReciver;
    // Use this for initialization
    void Start () {
        targetReciver = null;
        Transform seek = transform;
        while (seek != null)
        {
            Reciveration rtarget = seek.gameObject.GetComponent<Reciveration>();
            if (rtarget != null)
            {
                targetReciver = rtarget as CollisionReciver;
                eventObject = rtarget;
                //Debug.LogWarning(eventObject.name + ":target Reciver is yes");
                break;
            }
            /*Bonduration btarget = seek.gameObject.GetComponent<Bonduration>();
            Modurnation mtarget = seek.gameObject.GetComponent<Modurnation>();
            Combination ctarget = seek.gameObject.GetComponent<Combination>();

            if (btarget != null)
            {
                targetReciver = btarget as CollisionReciver;
                eventObject = btarget;
                break;
            }
            if (mtarget != null)
            {
                targetReciver = mtarget as CollisionReciver;
                eventObject = mtarget;
                break;
            }
            if (ctarget != null)
            {
                targetReciver = ctarget as CollisionReciver;
                eventObject = ctarget;
                break;
            }*/
            seek = seek.parent;
        }
        if(targetReciver == null)
        {
            name = "[empty reciver]" + name;
        }
       /* if (GetComponent<Collider2D>())
        {

            Collider2D tCollider = GetComponent<Collider2D>();
            if (!tCollider.isTrigger && GetComponent<Rigidbody2D>() == null)
            {
                Rigidbody2D rigid2D = gameObject.AddComponent<Rigidbody2D>();
                rigid2D.gravityScale = 0.0f;
                rigid2D.isKinematic = true;
            }
        }*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public bool AllowEvent(Collision2D target)
    {
        CollisionRecord tRecord = target.gameObject.GetComponent<CollisionRecord>();
        if (tRecord == null)
        {
            return false;
        }
        Reciveration other = target.gameObject.GetComponent<CollisionRecord>().eventObject;
        if (targetReciver == null)
        {
            Debug.LogWarning(name + ":target Reciver is null");
            return false;
        }
        return other != null && targetReciver.GetType() == other.GetType() && (other.parentReciveration == null || other.parentReciveration != eventObject.parentReciveration);
    }
    public bool AllowEvent(Collider2D target)
    {
        CollisionRecord tRecord = target.gameObject.GetComponent<CollisionRecord>();
        if(tRecord == null)
        {
            return false;
        }
        Reciveration other = tRecord.eventObject;
        if(targetReciver == null)
        {
            targetReciver = eventObject as CollisionReciver;
            //Debug.LogWarning(name + ":target Reciver is null");
            return false;
        }
        return other != null && targetReciver.GetType() == other.GetType() && (other.parentReciveration == null || other.parentReciveration != eventObject.parentReciveration);
    }

    void CallCollisionEvent(Collision2D tCollision,CollisionType type)
    {
        if (!AllowEvent(tCollision)) { return; }
        if (targetReciver == null) { return; }
        //Debug.LogWarning(tCollision.gameObject.name+" collision ");
        targetReciver.Collision(eventid, tCollision, type);
    }
    void CallTriggerEvent(Collider2D tCollider, CollisionType type)
    {
        if (!AllowEvent(tCollider)) { return; }
        if (targetReciver == null) { return; }
        targetReciver.Trigger(eventid, tCollider, type);
    }

    void OnCollisionEnter2D(Collision2D target){CallCollisionEvent(target, CollisionType.ENTER); }
    void OnCollisionExit2D(Collision2D target){ CallCollisionEvent(target, CollisionType.EXIT);}
    void OnTriggerEnter2D(Collider2D target){CallTriggerEvent(target, CollisionType.ENTER);}
    void OnTriggerExit2D(Collider2D target) {CallTriggerEvent(target, CollisionType.EXIT); }
}
