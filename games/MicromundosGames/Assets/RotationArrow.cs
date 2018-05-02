using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationArrow : TagController {

	public float speed;
	void Start()
	{
		GetComponent<Colorate> ().SetOn (id);
	}
	void Update () {
		Vector3 rot = transform.localEulerAngles;
		rot.z += Time.deltaTime * speed;
		transform.localEulerAngles = rot;
	}
}
