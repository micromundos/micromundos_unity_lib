using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour {

	public Camera cam;
	public List<TagController> tagList;

	List<TagController> instances;

	Dictionary<int,bool> detectedIds;

	// Use this for initialization
	void Start () {

		detectedIds = new Dictionary<int, bool> ();

		foreach(TagController tc in tagList)
			detectedIds [tc.id] = false;

		instances = new List<TagController> ();

		ClientEvents.OnBlockDetected += OnBlockDetected;
		ClientEvents.OnBlockExit += OnBlockExit;
	}

	void OnDestroy(){
		ClientEvents.OnBlockDetected -= OnBlockDetected;
		ClientEvents.OnBlockExit -= OnBlockExit;
	}

	void OnBlockDetected(int _id){
		foreach (TagController tc in tagList)
			if (tc.id == _id) {
				instances.Add(Instantiate (tc));
				detectedIds [_id] = true;
			}
				//tc.OnBlockDetected ();
	}

	void OnBlockExit(int _id){

		for (int i = instances.Count - 1; i >= 0; i--) {			
			if (instances [i].id == _id) {
				instances [i].OnBlockExit ();
				instances.Remove(instances [i]);
				detectedIds [_id] = false;
			}
		}
		/*foreach (TagController tc in instances)
			if (tc.id == _id) {
				tc.OnBlockExit ();
				instances.Remove(tc);
				detectedIds [_id] = false;
			}*/
	}

	void Update(){
		foreach (TagController tc in instances) {
			if (detectedIds [tc.id]) {
				tc.SetRotation ();
				tc.SetPosition (cam);
				tc.OnUpdate ();
			}
		}
	}
}
