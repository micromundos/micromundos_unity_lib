using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : TagController {
	void Start()
	{
		GetComponent<Colorate> ().SetOn (id);
	}
}
