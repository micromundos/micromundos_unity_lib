using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsCreator : TagController {

	void Start () {
		Loop ();
		GetComponent<Colorate> ().SetOn (id);
	}
	void Loop()
	{
		Game.Instance.AddCar (transform, id);
		Invoke ("Loop", 1);
	}
}
