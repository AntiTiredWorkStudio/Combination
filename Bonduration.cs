using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonduration : Reciveration
{
    public Animator targetAnimator;

    // Use this for initialization
    void Start ()
    {
        targetAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
