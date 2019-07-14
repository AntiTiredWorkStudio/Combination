using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BondState
{
    Connection,
    Connecting,
    Break
}
public class Bonduration : Reciveration
{
    public Animator targetAnimator;
    public Vector3 OppositePosition
    {
        get
        {
            Vector3 pos = transform.position + transform.up*0.05f;
            return pos;
        }
    }

    /*public Vector3 OppositeUpDirection
    {
        get
        {
            return -transform.up;
        }
    }*/

    public Vector3 OppositeRotateEularAngles
    {
        get
        {
            Vector3 current = transform.eulerAngles;
            Vector3 delta = new Vector3(0.0f, 0.0f, 180.0f);
            if(current.z > 180.0f)
            {
                delta.z = -180.0f;
            }
            else
            {
                delta.z = 180.0f;
            }

            current.x = 0.0f;
            current.y = 0.0f;
            current.z = (current.z + delta.z) % 360.0f;
            return current;
        }
    }

    public Bonduration destinyBonduration = null;
    public BondState bondState = BondState.Break;
    public SpriteRenderer bondurationRegion = null;

    // Update is called once per frame
    void FixedUpdate()
    {
        OnAttaching();
    }

    /// <summary>
    /// 开始吸引
    /// </summary>
    /// <param name="target"></param>
    public void StartAttach(Bonduration target)
    {
        if(dynamicState == DynamicState.Stable)
        {
            return;
        }
        if(HandleManager.Instance.handleTarget == RootReciveration)
        {
            foreach (GameObject t in GameObject.FindGameObjectsWithTag("viewregion"))
            {
                t.GetComponent<SpriteRenderer>().enabled = false;
            }
            HandleManager.Instance.handleTarget = null;
        }
        destinyBonduration = target;
        bondState = BondState.Connecting;
        RootReciveration.ColliderMonitor.MagicCollider(true);
        Timer.addTimer(gameObject,1.0f, ForceOnTime).startTimer();
#if UNITY_EDITOR
        UnityEditor.Selection.activeObject = this.gameObject;
#endif
    }

    public void ForceOnTime()
    {
        if (/*!ConnectionJurdgment &&*/ destinyBonduration != null)
        {
            TransformComplete(transform, destinyBonduration.OppositePosition, destinyBonduration.OppositeRotateEularAngles);
            if (bondState != BondState.Connection)
            {
                OnAttached(destinyBonduration);//自己连接完毕
            }
            if (destinyBonduration.bondState != BondState.Connection)
            {
                destinyBonduration.OnAttached(this);//对方连接完毕
            }
        }
    }

    public bool ConnectionJurdgment
    {
        get
        {
            if(destinyBonduration == null)
            {
                return false;
            }
            bool distance = Vector2.Distance(transform.position, destinyBonduration.OppositePosition) < 0.0005f;
            if(destinyBonduration == null)
            {
                BreakAttach(destinyBonduration);
            }
            return destinyBonduration != null && distance;
        }
    }

    public override bool CanHandle
    {
        get
        {
            return bondState != BondState.Connecting;
        }
    }

    /// <summary>
    /// 吸引中
    /// </summary>
    public void OnAttaching()
    {
        if(dynamicState == DynamicState.Stable)//静态不会被吸引
        {
            return;
        }
        if (bondState == BondState.Connecting && destinyBonduration != null && destinyBonduration.bondState == BondState.Break)//键的情况是断开且有目标键
        {

            //判断是否到达临界点
            if (ConnectionJurdgment)
            {
                //设定为终点
                TransformComplete(transform, destinyBonduration.OppositePosition, destinyBonduration.OppositeRotateEularAngles);
                OnAttached(destinyBonduration);//自己连接完毕
                destinyBonduration.OnAttached(this);//对方连接完毕
                SetDynamicState(DynamicState.Stable);//修改状态
            }
            else
            {
                //位移、旋转缓动
                TransformDelta(transform, destinyBonduration.OppositePosition, destinyBonduration.OppositeRotateEularAngles);
                //Debug.Log("not in destiny:" + transform.position + "," + destinyBonduration.OppositePosition + ","+ Vector3.Distance(transform.position, destinyBonduration.OppositePosition));
                Debug.DrawLine(transform.position, destinyBonduration.OppositePosition,Color.red);
            }
        }
    }

    /// <summary>
    /// 吸引成功时
    /// </summary>
    public void OnAttached(Bonduration other)
    {
        if(dynamicState == DynamicState.Stable)
        {
            destinyBonduration = other;
        }
        bondState = BondState.Connection;
        RootReciveration.ColliderMonitor.MagicCollider(false);
        targetAnimator.SetBool("GateOpen", true);
        Debug.Log(RootReciveration.name + " on attached:" + other.RootReciveration.name);
    }

    /// <summary>
    /// 结束吸引
    /// </summary>
    public void EndAttach()
    {
        if(destinyBonduration!= null)
        {
            destinyBonduration.destinyBonduration = null;
            destinyBonduration.bondState = BondState.Break;
            destinyBonduration = null;
            bondState = BondState.Break;
        }
    }

    /// <summary>
    /// 断开吸引
    /// </summary>
    /// <param name="target"></param>
    public void BreakAttach(Bonduration target)
    {
        if(target == destinyBonduration && destinyBonduration!=null)
        {
            if (destinyBonduration == this)
            {
                destinyBonduration = null;
            }
            else
            {
                if (dynamicState == DynamicState.Dynamic)
                    destinyBonduration.BreakAttach(this);
                Debug.LogWarning(RootReciveration.name + " Break Attach " + destinyBonduration.RootReciveration.name);
                destinyBonduration = null;
            }
        }
        bondState = BondState.Break;
        targetAnimator.SetBool("GateOpen", false);
    }

    public override void Collision(string eid, Collision2D target, CollisionType type)
    {
        Debug.LogWarning(eid + "," + target.collider.name + "," + type);
    }

    public override void Init()
    {
        targetAnimator = GetComponent<Animator>();
        bondurationRegion = transform.Find("DetectRegion").gameObject.GetComponentInChildren<SpriteRenderer>();
        bondurationRegion.color = new Color(0.35f,1.0f, 0.1f, 0.45f);
        bondurationRegion.enabled = false;
        bondurationRegion.gameObject.tag = "viewregion";
    }

    /// <summary>
    /// 当准备链接，显示触碰范围
    /// </summary>
    public void Focus(bool state)
    {
        if (bondState == BondState.Break)
        {
            bondurationRegion.color = new Color(0.35f, 1.0f, 0.1f, 0.45f);//断开时显示的颜色
        }
        else
        {
            bondurationRegion.color = new Color(0.7f, 0.7f, 0.7f, 0.45f);//链接时的颜色
        }
        bondurationRegion.enabled = state;
    }

    public override void Trigger(string eid, Collider2D target, CollisionType type)
    {
        if (dynamicState == DynamicState.Stable)
        {
            return;
        }
        Bonduration tBond = CollisionRecord.TransforReciver<Bonduration>(target);
        if(tBond.dynamicState == DynamicState.Dynamic)
        {
            return;
        }
        if (tBond == null)
        {
            return;
        }
        if (type == CollisionType.ENTER) {
            if(bondState == BondState.Break && tBond.bondState == BondState.Break && AttachCondition(tBond))//
            {
                StartAttach(tBond);
            }
        }
        else if(type == CollisionType.EXIT)
        {
            if (bondState == tBond.bondState && bondState == BondState.Connection)
            {
                if(tBond == destinyBonduration)
                {
                    destinyBonduration.BreakAttach(this);
                    BreakAttach(tBond);
                }
            }
            else if (bondState == tBond.bondState && bondState == BondState.Connecting)
            {
                EndAttach();
            }
        }
    }

    /// <summary>
    /// 判断与该键所在物体所有键是否有键已经和目标键连接
    /// </summary>
    /// <param name="bond"></param>
    /// <returns></returns>
    bool AttachCondition(Bonduration bond)
    {
        bool result = true;
        bool result01 = true;
        foreach (Bonduration tbond in GetSelfSingleLevelReciveration<Bonduration>())
        {
            result01 = result01 && (tbond == this || tbond.bondState != BondState.Connecting);
            if (tbond.destinyBonduration == null)
            {
                continue;
            }
            result = result && (tbond.destinyBonduration.RootReciveration != bond.RootReciveration) ;
            //Debug.LogWarning("result:"+result);
        }
        return result && result01;
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawCube(OppositePosition, Vector3.one * 0.06f);
    }

    public override void OnDynamicStateChange(DynamicState cState)
    {
        //Debug.LogWarning(name+" OnDynamicStateChange "+cState+ " "+bondState);
        if (cState == DynamicState.Stable && bondState == BondState.Connection)
        {
            //Debug.LogWarning(name + "TransformComplete");
            //Debug.LogWarning(name + "ConnectionJurdgment");
            /*if (!ConnectionJurdgment)
            {
                destinyBonduration.BreakAttach(this);
                BreakAttach(destinyBonduration);
            }else*/
            TransformComplete(transform, destinyBonduration.OppositePosition, destinyBonduration.OppositeRotateEularAngles);
        }

        if(cState == DynamicState.Dynamic && (bondState == BondState.Connection || bondState == BondState.Connecting) )
        {
            //Debug.LogWarning(name + "ConnectionJurdgment");
            if (!ConnectionJurdgment)
            {
               // Debug.LogWarning(name + "ConnectionJurdgment Break");
                BreakAttach(destinyBonduration);
            }
        }
    }

    public override void UpdateState()
    {
        if (!ConnectionJurdgment)
        {
            dynamicState = DynamicState.Stable;
            bondState = BondState.Break;
            destinyBonduration = null;
            targetAnimator.SetBool("GateOpen", false);
        }
    }

    public override void Actions()
    {

    }
}
