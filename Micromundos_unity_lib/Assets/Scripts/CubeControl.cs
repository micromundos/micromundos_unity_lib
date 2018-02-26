using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControl : MonoBehaviour {

	public int id;
	public float scale;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (MicromundosManager.Instance.msgClient.GetBlock (id) != null) {
			
			transform.eulerAngles = new Vector3 (0f, 0f, Mathf.Rad2Deg * MicromundosManager.Instance.msgClient.GetBlock (id).angle_i);
			//transform.eulerAngles = new Vector3 (0f, 0f, Vector2.Angle (Vector2.zero, ClientData.Instance.msgClient.GetBlock (id).dir_i));
			transform.position = new Vector3 ((MicromundosManager.Instance.msgClient.GetBlock (id).loc_i.x-0.5f)*scale,
				(1f-MicromundosManager.Instance.msgClient.GetBlock (id).loc_i.y)*scale-0.5f*scale,
				0f);
		}
	}
}
