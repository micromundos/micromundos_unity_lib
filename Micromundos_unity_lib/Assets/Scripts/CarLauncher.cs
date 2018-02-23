using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLauncher : MonoBehaviour {

	public int id;
	public Camera cam;
	public GameObject car;
	public float interval;

	float t;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (ClientData.Instance.IsBlock(id)) {

			transform.eulerAngles = ClientData.Instance.GetBlockRotation(id);

			transform.localPosition = ClientData.Instance.GetBlockPositionAtNearPlane (id, cam);

			t += Time.deltaTime;
			if (t > interval) {
				ShotCar ();
				t=0;
			}
		}
	}

	void ShotCar(){
		GameObject c = Instantiate (car);
		c.transform.eulerAngles = transform.eulerAngles;
		c.transform.position = transform.position;
	}
}
