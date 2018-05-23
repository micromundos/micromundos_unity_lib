using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsTagController : TagController {

	public int carID;
	public void SetCarID(int _carID)
	{
		this.carID = _carID;
		Colorate c = GetComponent<Colorate> ();
		if (c != null)
			c.SetOn (carID);
	}
}
