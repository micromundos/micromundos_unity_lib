using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : TagController {

	public int num;
	public GameObject fieldsContainer;

	void Start()
	{
		GetComponent<Colorate> ().SetOn (carId);
	}
	public void SetOn(int _carId)
	{
		if(carId != _carId) return;
		NewCar ();
		GetComponent<Colorate> ().SetOn (carId);
	}
	void NewCar () {
		num++;
		foreach (TextMesh t in fieldsContainer.GetComponentsInChildren<TextMesh>())
		{
			t.text = num.ToString();
		}
	}
}
