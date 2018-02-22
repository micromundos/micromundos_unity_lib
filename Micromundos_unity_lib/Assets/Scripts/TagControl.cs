using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagControl : MonoBehaviour {

	public int id;
	public Camera cam;
	public GameObject car;
	public float interval;

	ParticleSystem ps;

	float t;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
		ClientEvents.OnBlockDetected += OnBlockDetected;
		ClientEvents.OnBlockExit += OnBlockExit;
	}

	void OnDestroy(){
		ClientEvents.OnBlockDetected -= OnBlockDetected;
		ClientEvents.OnBlockExit -= OnBlockExit;
	}

	void OnBlockDetected(int _id){
		if (_id == id)
			ps.Play ();
	}

	void OnBlockExit(int _id){
		if (_id == id)
			ps.Stop ();
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
