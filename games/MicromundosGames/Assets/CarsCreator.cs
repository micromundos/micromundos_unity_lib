using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsCreator : TagController {

	void Start () {
		Loop ();
		GetComponent<Colorate> ().SetOn (carId);
	}
	void Loop()
	{
		Game.Instance.AddCar (transform, carId);
		Invoke ("Loop", 1);
	}
}
