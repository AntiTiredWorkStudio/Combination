using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CollisionType
{
    ENTER,EXIT
}
public interface CollisionReciver
{
    void Collision(string eid,Collision target, CollisionType type);
    void Trigger(string eid,Collider target, CollisionType type);
}
public class CollisionRecord : MonoBehaviour {
    public string eventid;
    public GameObject eventObject;
    CollisionReciver targetReciver;
    // Use this for initialization
    void Start () {
        targetReciver = null;
        Transform seek = transform;
        while (seek != null)
        {
            Bonduration btarget = seek.gameObject.GetComponent<Bonduration>();
            Modurnation mtarget = seek.gameObject.GetComponent<Modurnation>();
            Combination ctarget = seek.gameObject.GetComponent<Combination>();
            //Debug.Log(btarget);
            if (btarget != null)
            {
                targetReciver = btarget as CollisionReciver;
                eventObject = btarget.gameObject;
                break;
            }
            if (mtarget != null)
            {
                targetReciver = mtarget as CollisionReciver;
                eventObject = mtarget.gameObject;
                break;
            }
            if (ctarget != null)
            {
                targetReciver = ctarget as CollisionReciver;
                eventObject = ctarget.gameObject;
                break;
            }
            seek = seek.parent;
        }
        if(targetReciver == null)
        {
            name = "[empty reciver]" + name;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void CallEvent()
    {
        
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        Debug.Log(gameObject.name+" c enter:"+target.gameObject.name);
    }

    void OnCollisionExit2D(Collision2D target)
    {
        Debug.Log(gameObject.name + " c exit:" + target.gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        Debug.Log(gameObject.name + " t enter:" + target.gameObject.name);
    }

    void OnTriggerExit2D(Collider2D target)
    {
        Debug.Log(gameObject.name + " t exit:" + target.gameObject.name);
    }
}
