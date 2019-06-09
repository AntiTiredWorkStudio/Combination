using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameServer : MonoBehaviour {

#if UNITY_EDITOR
    [ContextMenu("Build Bundle")]
    public void BuildBundle()
    {

        UnityEditor.BuildPipeline.BuildAssetBundles("Assets", UnityEditor.BuildAssetBundleOptions.CollectDependencies,UnityEditor.BuildTarget.StandaloneWindows64);

        UnityEditor.AssetDatabase.Refresh();

        //        AssetBundle.
    }

    [ContextMenu("Collection")]
#endif
    public void CollectModurnation()
    {
        foreach (Modurnation target in GameObject.FindObjectsOfType<Modurnation>())
        {
            if (target.transform.parent != null)
            {
                continue;
            }
            GameObject combination = new GameObject("Combination_" + target.gameObject.name);
            CircleSearch(target, combination.transform);
            if (combination.transform.childCount == 0)
            {
                Destroy(combination);
            }
            else
            {
                CenterPivot(combination.transform);
                Combination CombinationTarget = combination.AddComponent<Combination>();
                CombinationTarget.Init();
            }
        }
    }

    void CenterPivot(Transform target)
    {
        Vector3 center = Vector3.zero;
        //Bounds bounds = new Bounds(center, Vector3.zero);
        Renderer[] rendList = target.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in rendList)
        {
            center+=rend.bounds.center;
        }
        if (rendList.Length > 0)
        {
            center /= rendList.Length;

            Vector3 delta = target.transform.position - center;
            foreach (Transform trans in target.GetComponentsInChildren<Transform>())
            {
                if(trans.parent != target)
                {
                    continue;
                }
                trans.position = (trans.position + delta);
            }
            target.transform.position -= delta;
        }

       /* foreach (Transform trans in target.GetComponentsInChildren<Transform>())
        {
            if (!trans.IsChildOf(target))
            {
                continue;
            }
            trans.position += (trans.position - center);
        }*/
    }

    void CircleSearch(Modurnation dest,Transform parent)
    {
        foreach (Bonduration bond in dest.transform.GetComponentsInChildren<Bonduration>())
        {
            if (bond.bondState != BondState.Connection || bond.destinyBonduration == null || bond.destinyBonduration.destinyBonduration == null)
            {
                continue;
            }
            if (bond.parentReciveration.transform.parent != parent)
            {
                bond.parentReciveration.transform.parent = parent;
                CircleSearch(bond.parentReciveration as Modurnation, parent);
            } else
            if(bond.destinyBonduration.parentReciveration.transform.parent != parent)
            {
                bond.destinyBonduration.parentReciveration.transform.parent = parent;
                CircleSearch(bond.destinyBonduration.parentReciveration as Modurnation, parent);
            }
            else
            {
                continue;
            }
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
