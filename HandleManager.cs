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
    Vector3 offsetDragPos;
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
                handleTarget.SetDynamicState(DynamicState.Dynamic);
                

                Vector3 current = hit2D.point;
                CursorSelection.position = current;
                offsetDragPos = CursorSelection.position - handleTarget.transform.position;
                foreach(GameObject target in GameObject.FindGameObjectsWithTag("viewregion"))
                {
                    target.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
        if(handleTarget != null)
        {
            Vector3 current = InputPosition - offsetDragPos;
            current.z = 0.0f;
            CursorSelection.position = current;
            handleTarget.TransformComplete(CursorSelection, CursorSelection.position, handleTarget.transform.eulerAngles);
        }


        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(handleTarget != null)
            {
                handleTarget.SetDynamicState(DynamicState.Stable);

                handleTarget = null;

                foreach (GameObject target in GameObject.FindGameObjectsWithTag("viewregion"))
                {
                    target.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }


}
