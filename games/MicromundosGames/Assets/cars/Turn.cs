using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : CarsTagController {
	void Start()
	{
		GetComponent<Colorate> ().SetOn (id);
	}
}
