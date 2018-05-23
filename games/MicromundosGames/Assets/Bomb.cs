using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : CarsTagController {


	void Start()
	{
		GetComponent<Colorate> ().SetOn (id);
	}
}
