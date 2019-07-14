using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class HandleManager : MonoBehaviour {
    public static HandleManager Instance
    {
        get
        {
            return GameObject.FindObjectOfType<HandleManager>();
        }
    }
    public Vector3 DragOffset
    {
        get
        {
            return offsetDragPos + hitOffsetDragPos;
        }
    }
    public Vector3 offsetDragPos;
    public Vector3 hitOffsetDragPos = Vector3.zero;
    public Transform CursorSelection;
    public Camera InputCamera;
    public Reciveration handleTarget = null;
	// Use this for initialization
	void Start () {
        CursorSelection = (new GameObject("CursorSelection")).transform;
        CursorSelection.gameObject.transform.parent = transform;
        InputCamera = GetComponent<Camera>();
    }
	
    public Vector3 InputPosition
    {
        get
        {
            Vector3 targetPos = InputCamera.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0.0f;
            return targetPos;
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(InputPosition, Vector3.forward);
            if (hit2D)
            {
                //Debug.LogWarning(hit2D+":"+hit2D.collider.name);
                handleTarget = CollisionRecord.TransforReciver<Reciveration>(hit2D.collider);

                //Debug.Log("click:" + handleTarget.RootReciveration.CanHandle);
                if (!handleTarget.RootReciveration.CanHandle)
                {
                    handleTarget.RootReciveration.UpdateState();
                    /*Debug.Log("无法移动" + handleTarget.RootReciveration.CanHandle);
                    handleTarget = null;
                    return;*/
                }
                handleTarget.SetDynamicState(DynamicState.Dynamic);
                

                Vector3 current = hit2D.point;
                CursorSelection.position = current;
                offsetDragPos = CursorSelection.position - handleTarget.transform.position;
                hitOffsetDragPos = Vector3.zero;
                HintRegionView(true);
            }
        }
        if(handleTarget != null)
        {
            Vector3 current = InputPosition - DragOffset;
            current.z = 0.0f;
            CursorSelection.position = current;
            if(handleTarget.dynamicState!=DynamicState.Dynamic)
                handleTarget.SetDynamicState(DynamicState.Dynamic);
            handleTarget.TransformComplete(CursorSelection, CursorSelection.position, handleTarget.transform.eulerAngles);
        }


        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (handleTarget != null)
            {
                handleTarget.SetDynamicState(DynamicState.Stable);

                handleTarget = null;

            }
            HintRegionView(false);
        }
    }

    public void HintRegionView(bool result)
    {
        foreach (GameObject target in GameObject.FindGameObjectsWithTag("viewregion"))
        {
            CollisionRecord collisionRec = target.GetComponentInParent<CollisionRecord>();
            if (collisionRec != null)
            {
                Bonduration bonduration = collisionRec.eventObject.ToReciveration<Bonduration>();

                Modurnation module = null;
                if (handleTarget != null) {
                    module = handleTarget.ToReciveration<Modurnation>();
                }
                
                if (module && module.bodyCollisions.CollisionMotion(bonduration.OppositePosition, bonduration.OppositeRotateEularAngles.z))
                {

                }else
                bonduration.Focus(result);
            }
        }
    }

}
