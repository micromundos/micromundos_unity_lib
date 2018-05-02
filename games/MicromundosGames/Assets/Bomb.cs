using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : TagController {


	void Start()
	{
		GetComponent<Colorate> ().SetOn (id);
	}
}
