using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DynamicState
{
    Stable,//稳定
    Dynamic//动态
}
public abstract class Reciveration : MonoBehaviour ,CollisionReciver {

    [ContextMenu("Dynamic All")]
    public void SetDynamic()
    {
        SetDynamicState(DynamicState.Dynamic);
    }

    [ContextMenu("Stable All")]
    public void SetStable()
    {
        SetDynamicState(DynamicState.Stable);
    }

    public Reciveration parentReciveration = null;

    public DynamicState dynamicState = DynamicState.Stable;

    /// <summary>
    /// 获取根节点的接收器
    /// </summary>
    public Reciveration RootReciveration
    {
        get
        {
            Reciveration seek = this;
            while (seek != null)
            {
                if (seek.parentReciveration == null)
                {
                    break;
                }
                seek = seek.parentReciveration;
            }
            return seek;
        }
    }
    /// <summary>
    /// 设置组合的交互状态
    /// </summary>
    /// <param name="tState"></param>
    public void SetDynamicState(DynamicState tState) {

        foreach(Reciveration reciver in RootReciveration.gameObject.GetComponentsInChildren<Reciveration>())
        {
            reciver.dynamicState = tState;
            reciver.OnDynamicStateChange(tState);
        }
    }

    /// <summary>
    /// 当动态情况被改变
    /// </summary>
    /// <param name="cState"></param>
    public abstract void OnDynamicStateChange(DynamicState cState);

    /// <summary>
    /// 获取当前物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T[] GetSelfSingleLevelReciveration<T>() where T:Reciveration
    {
        return RootReciveration.gameObject.GetComponentsInChildren<T>();
    }

    public abstract void Collision(string eid, Collision2D target, CollisionType type);

    public abstract void Trigger(string eid, Collider2D target, CollisionType type);

    public abstract void Init();


    public void TransformDelta(Transform sub,Vector3 tPosition,Vector3 tEularAngles)
    {
        if (parentReciveration == null)
        {
            transform.position = Vector3.Lerp(transform.position, tPosition, Time.deltaTime * 10.0f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, tEularAngles, Time.deltaTime * 10.0f);
        }
        else
        {
            Vector3 deltaPos = transform.position - parentReciveration.transform.position;
            Vector3 deltaUp = parentReciveration.transform.eulerAngles - transform.eulerAngles;
            parentReciveration.TransformDelta(transform, tPosition - deltaPos, tEularAngles + deltaUp);
        }
    }

    public void TransformComplete(Transform sub, Vector3 tPosition, Vector3 tEularAngles)
    {
        if (parentReciveration == null)
        {
            //Debug.LogWarning("Complete:" + name);
            transform.position = tPosition;
            transform.eulerAngles = tEularAngles;
            tEularAngles.Set(0.0f, 0.0f, tEularAngles.z);
        }
        else
        {
            Vector3 deltaPos = transform.position - parentReciveration.transform.position;
            Vector3 deltaUp = parentReciveration.transform.eulerAngles - transform.eulerAngles;
            parentReciveration.TransformComplete(transform, tPosition - deltaPos, tEularAngles + deltaUp);
        }
    }

    // Use this for initialization
    void Start () {
        Vector3 eurlarAngles = transform.eulerAngles;
        eurlarAngles.Set(0.0f, 0.0f, eurlarAngles.z);
        transform.eulerAngles = eurlarAngles;
        Init();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 eurlarAngles = transform.eulerAngles;
        eurlarAngles.Set(0.0f, 0.0f, eurlarAngles.z);
        transform.eulerAngles = eurlarAngles;
    }
}
