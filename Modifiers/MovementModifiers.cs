using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModifiers : Modifiers
{
    public Vector2 lastPosition;
    public Vector2 deltaPos;
    public Vector2 direction
    {
        get
        {
            return deltaPos.normalized;
        }
    }

    public float deltaStrength
    {
        get
        {
            return deltaPos.magnitude;
        }
    }
	// Use this for initialization
	void Start () {
        lastPosition = transform.position;
        deltaPos = Vector2.zero;

    }
	
	// Update is called once per frame
	void Update () {
        Vector2 currentPos = transform.position;
        deltaPos = Vector2.Lerp(deltaPos, currentPos - lastPosition,0.2f);
        lastPosition = transform.position;
	}

    public RaycastHit2D MakeRaycast()
    {
        RaycastHit2D[] dRays = Physics2D.RaycastAll(lastPosition, direction);
        foreach(RaycastHit2D ray2D in dRays)
        {
            if(ray2D.collider == null)
            {
                continue;
            }
            CollisionRecord cRecord = ray2D.collider.gameObject.GetComponent<CollisionRecord>();
            if (cRecord!= null)
            {
                if(cRecord.eventid == "Attract" && cRecord.eventObject.RootReciveration!=ReciverationObject.RootReciveration)
                {
                    Debug.DrawRay(lastPosition, direction * 100.0f, Color.red);
                    Debug.LogWarning(ray2D.collider);
                    /*Vector3 targetAngle = ReciverationObject.transform.eulerAngles;
                    float z = Vector3.Angle(new Vector3(0.0f, 1.0f, 0.0f), (ReciverationObject.transform.position- cRecord.eventObject.transform.position).normalized);
                    targetAngle.z = 180.0f+z;*/
                    ReciverationObject.RootReciveration.transform.eulerAngles =Vector3.Lerp(ReciverationObject.RootReciveration.transform.eulerAngles,  (cRecord.eventObject as Bonduration).OppositeRotateEularAngles,Time.deltaTime*20.0f);
                  //Debug.LogWarning("Angle:" + z);

//                    ReciverationObject.RootReciveration.tra
                    return ray2D;
                }
            }
        }
        return new RaycastHit2D();
    }
}
