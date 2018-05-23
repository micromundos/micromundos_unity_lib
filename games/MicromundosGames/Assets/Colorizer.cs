using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorizer : CarsTagController {
	 
	void Start () {		
		GetComponent<Colorate> ().SetOn (carID);
	}
	void OnTriggerEnter2D(Collider2D other)
	{		
		TagController tc = other.GetComponent<TagController> ();
		if (tc == null)
			return;
		if (other.GetComponent<Colorizer> ())
			return;
		CarsTagController carsTagController = other.GetComponent<CarsTagController> () ;
		if (carsTagController == null)
			return;
		
		carsTagController.SetCarID (carID);

		//SetCarID (carID);
	}
//	void SetCarID(int _carId)
//	{
//		this.carID = _carId;
//		Colorate c = GetComponent<Colorate> ();
//		if (c != null)
//			c.SetOn (id);		
//	}
}
