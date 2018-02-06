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
		if (ClientData.Instance.msgClient.GetBlock (id) != null) {
			transform.eulerAngles = new Vector3 (0f, 0f, Mathf.Rad2Deg * ClientData.Instance.msgClient.GetBlock (id).angle_i);
			transform.position = new Vector3 (ClientData.Instance.msgClient.GetBlock (id).loc_i.x*scale,ClientData.Instance.msgClient.GetBlock (id).loc_i.y*scale,0f);
		}
	}
}
