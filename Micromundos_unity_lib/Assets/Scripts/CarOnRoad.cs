using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOnRoad : MonoBehaviour {

	BoxCollider bcol;
	BinControl control;

	// Use this for initialization
	void Start () {
		bcol = GetComponent<BoxCollider> ();
		control = GetComponent<BinControl> ();
	}

	void OnTriggerEnter(Collider other){
		control.onRoad = true;
		if (other.tag == "road") {
			control.SetDir(other.transform.parent.transform.parent.eulerAngles);
		}
	}

	void OnTriggerExit(Collider other){
		control.onRoad = false;
	}

	void TriggOff(){
		bcol.isTrigger = false;
	}

	void TriggOn(){

	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
