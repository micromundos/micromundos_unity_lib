using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagControl : MonoBehaviour {

	public int id;
	public GameObject assets;


	// Use this for initialization
	void Start () {
		ClientEvents.OnBlockDetected += OnBlockDetected;
		ClientEvents.OnBlockExit += OnBlockExit;
	}

	void OnDestroy(){
		ClientEvents.OnBlockDetected -= OnBlockDetected;
		ClientEvents.OnBlockExit -= OnBlockExit;
	}

	void OnBlockDetected(int _id){
		if (_id == id)
			assets.SetActive (true);
	}

	void OnBlockExit(int _id){
		if (_id == id)
			assets.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
	}

}
