using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnTouch : MonoBehaviour {

	Animation anim;
	void Start()
	{
		anim = GetComponent<Animation> ();
	}
	public void SetOn()
	{
		if(anim !=null)
			anim.Play ("onActive");
	}
}
