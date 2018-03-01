using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorizer : TagController {
	 
	void Start () {		
		GetComponent<Colorate> ().SetOn (carId);
	}
	void OnTriggerEnter2D(Collider2D other)
	{		
		print (other);
		TagController tc = other.GetComponent<TagController> ();
		if (tc == null)
			return;
		if (other.GetComponent<Colorizer> ())
			return;
		
		tc.SetCarID (carId);
	}
}
