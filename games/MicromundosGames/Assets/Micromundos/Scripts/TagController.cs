using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagController : MonoBehaviour {

	public int id;

	// Use this for initialization
	void Start () {
	}

	public void OnBlockDetected(){
		/*detected = true;
		Instantiate (gameObject);*/
	}

	public void OnBlockExit(){
		print ("tt");
		Destroy (this.gameObject);
	}

	public void SetRotation(){
			transform.eulerAngles = MicromundosManager.Instance.GetBlockRotation(id);
	}

	public void SetPosition(Camera cam){
			transform.localPosition = MicromundosManager.Instance.GetBlockPosition (id, cam);
		//transform.localPosition = MicromundosManager.Instance.GetBlockPositionAtZ (id, cam,0);
		//transform.localPosition = MicromundosManager.Instance.GetBlockPositionAtNearPlane (id, cam);
		//transform.localPosition = MicromundosManager.Instance.GetBlockPositionAtFarPlane (id, cam);
	}
	
	// Update is called once per frame
	public void OnUpdate () {
		
	}
}
