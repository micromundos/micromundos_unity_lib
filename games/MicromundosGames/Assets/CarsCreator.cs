using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsCreator : CarsTagController {

	void Start () {
		Loop ();
		GetComponent<Colorate> ().SetOn (carID);
	}
	void Loop()
	{
		bool canAdd = Game.Instance.AddCar (transform, carID);

		if (canAdd)
			GetComponent<Animation> ().Play ("onActive");
		
		Invoke ("Loop", 1);
	}
}
