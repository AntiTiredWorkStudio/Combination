using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers : MonoBehaviour {

    public static T AddModifiers<T>(Reciveration reciver) where T:Modifiers
    {
        T modifiers = reciver.gameObject.AddComponent<T>();
        modifiers.SelfInit(reciver);
        return modifiers;
    }

    protected Reciveration ReciverationObject;
    public void SelfInit(Reciveration targetEventReciver)
    {
        ReciverationObject = targetEventReciver;
    }

}
