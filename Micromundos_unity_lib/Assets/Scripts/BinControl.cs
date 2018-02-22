﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinControl : MonoBehaviour {

	public float speed;
	Rigidbody rb;
	Vector3 dir;

	bool detected;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		dir = Vector3.right;
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		/*if (Input.GetMouseButtonDown (0)) {
			Vector3 pos =Input.mousePosition;
			pos.z = 10;
			ClientData.Instance.GetObstacleAt (Camera.main.ScreenToWorldPoint(pos));
		}*/

		if (!detected && ClientData.Instance.GetObstacleAt (transform.position)) {
			dir = new Vector3 (dir.x * -1.1f, dir.y * -1.1f, 0f);
			transform.Rotate (180, 180, 0);
			detected = true;
		} else {
			detected = false;
		}

		Vector3 mov = dir.normalized * speed * Time.deltaTime;
		rb.MovePosition (transform.position + mov);


	}

}
