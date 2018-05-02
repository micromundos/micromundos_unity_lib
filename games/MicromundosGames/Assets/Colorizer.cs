using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorizer : TagController {
	 
	void Start () {		
		GetComponent<Colorate> ().SetOn (id);
	}
	void OnTriggerEnter2D(Collider2D other)
	{		
		TagController tc = other.GetComponent<TagController> ();
		if (tc == null)
			return;
		if (other.GetComponent<Colorizer> ())
			return;
		
		SetCarID (id);
	}
	void SetCarID(int _carId)
	{
		this.id = _carId;
		Colorate c = GetComponent<Colorate> ();
		if (c != null)
			c.SetOn (id);			
	}
}
