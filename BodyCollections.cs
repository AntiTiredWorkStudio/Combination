using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BodyCollision
{
    public Transform target;
    public List<CollisionRecord> bodyCollisions;
    public BodyCollision(Transform _tObject, List<CollisionRecord>  bodys)
    {
        target = _tObject;
        bodyCollisions = bodys;
        InitBound();
    }

    public BodyCollision(Transform _tObject,string targetid)
    {
        target = _tObject;
        bodyCollisions = CollisionRecord.GetCollisionById(target,targetid);
        InitBound();
    }

    public float MaxX = Mathf.NegativeInfinity;
    public float MaxY = Mathf.NegativeInfinity;
    public float MinX = Mathf.Infinity;
    public float MinY = Mathf.Infinity;

    public Vector2 BodySize
    {
        get
        {
            return new Vector2(MaxX - MinX, MaxY - MinY);
        }
    }

    public bool CollisionMotion(Vector3 point,float angle)
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(point, BodySize, angle,Vector2.zero);
        BodyCollision.DebugPoint(point,Color.magenta);
        if (hit2D.collider != null)
        {
            CollisionRecord cRec = hit2D.collider.GetComponent<CollisionRecord>();
            if(cRec != null && cRec.eventid == "body")
            {
                Debug.LogWarning(hit2D.collider.name);
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public Vector3 Center
    {
        get
        {
            return target.position + new Vector3(
            MinX + (MaxX - MinX) * 0.5f, MinY + (MaxY - MinY) * 0.5f
            );
        }
    }
    public void InitBound()
    {
          MaxX = Mathf.NegativeInfinity;
          MaxY = Mathf.NegativeInfinity;
          MinX = Mathf.Infinity;
          MinY = Mathf.Infinity;
        foreach (CollisionRecord crecd in bodyCollisions)
        {
            Vector3 originMax = crecd.selfCollider.bounds.max;
            Vector3 originMin = crecd.selfCollider.bounds.min;
            //crecd.selfCollider.bounds.extents

            MaxX = Mathf.Max(originMax.x, MaxX);// - crecd.transform.position.x;
            MaxY = Mathf.Max(originMax.y, MaxY); //- crecd.transform.position.y;
            MinX = Mathf.Min(originMin.x, MinX);// - crecd.transform.position.x;
            MinY = Mathf.Min(originMin.y, MinY);// - crecd.transform.position.y;
        }
        MaxX -= target.position.x;
        MaxY -= target.position.y;
        MinX -= target.position.x;
        MinY -= target.position.y;
    }



    public void Bound()
    {
        Vector3 oMax = target.position + new Vector3(MaxX,MaxY);
        Vector3 oMin = target.position + new Vector3(MinX, MinY);
        Vector3 a = target.position + new Vector3(MaxX, MinY);
        Vector3 b = target.position + new Vector3(MinX, MaxY);
        Debug.DrawLine(oMax, a);
        Debug.DrawLine(oMax, b);
        Debug.DrawLine(oMin, a);
        Debug.DrawLine(oMin, b);
        DebugPoint(oMax, Color.red);
        DebugPoint(oMin, Color.green);
        DebugPoint(a, Color.blue);
        DebugPoint(b, Color.yellow);
        DebugPoint(Center,Color.black);
    }

    public static void DebugPoint(Vector3 point,Color tColor)
    {
        Debug.DrawLine(point - Vector3.one * 0.05f, point + Vector3.one * 0.05f, tColor);
        Debug.DrawLine(point + (new Vector3(-1.0f, 1.0f)) * 0.05f, point + (new Vector3(1.0f, -1.0f)) * 0.05f, tColor);
    }
}
public class BodyCollections : MonoBehaviour {
    public List<BodyCollision> BodyCollisions;
	// Use this for initialization
	void Start () {
        InitAllBodyObjects();
    }

    public void InitAllBodyObjects()
    {
        BodyCollisions = new List<BodyCollision>();
        List<Modurnation> modurnations = new List<Modurnation>(GameObject.FindObjectsOfType<Modurnation>());
        foreach(Modurnation  module in modurnations)
        {
            BodyCollisions.Add(new BodyCollision(module.transform, CollisionRecord.GetCollisionById(module.transform, "body")));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
