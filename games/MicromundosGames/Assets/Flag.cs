using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : CarsTagController {

	public int num;
	public GameObject fieldsContainer;

	void Start()
	{
		GetComponent<Colorate> ().SetOn (id);
	}
	public void SetOn(int _carId)
	{
		if(carID != _carId) return;
		NewCar ();
		GetComponent<Colorate> ().SetOn (carID);
	}
	void NewCar () {
		num++;
		foreach (TextMesh t in fieldsContainer.GetComponentsInChildren<TextMesh>())
		{
			t.text = num.ToString();
		}
	}
}
