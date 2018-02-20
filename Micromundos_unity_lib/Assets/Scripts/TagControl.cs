using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagControl : MonoBehaviour {

	public int id;
	public Camera cam;

	public float angle;
	//Vector2 size;

	// Use this for initialization
	void Start () {
		/*RectTransform rt = transform.parent.transform as RectTransform;
		if (rt != null) {
			size.x = rt.rect.width;
			size.y = rt.rect.height;
		} else {
			size = new Vector2 (transform.parent.transform.lossyScale.x, transform.parent.transform.lossyScale.y);
		}*/
	}

	// Update is called once per frame
	void Update () {
		if (ClientData.Instance.msgClient.GetBlock (id) != null) {

			transform.eulerAngles = new Vector3 (0f, 0f, Mathf.Rad2Deg * ClientData.Instance.msgClient.GetBlock (id).angle_i);
			//angle = Vector2.Angle (Vector2.zero, ClientData.Instance.msgClient.GetBlock (id).dir_i);
			//transform.eulerAngles = new Vector3 (0f, 0f, angle);
			/*transform.position = new Vector3 ((ClientData.Instance.msgClient.GetBlock (id).loc_i.x-0.5f)*scale,
				(1f-ClientData.Instance.msgClient.GetBlock (id).loc_i.y)*scale-0.5f*scale,
				0f);*/

			transform.localPosition = cam.ViewportToWorldPoint (new Vector3 (ClientData.Instance.msgClient.GetBlock (id).loc_i.x,
				1f-ClientData.Instance.msgClient.GetBlock (id).loc_i.y,
				cam.farClipPlane));
		}

		/*transform.localPosition = new Vector3 (testpos.x*size.x-0.5f*size.x,
			(1f-testpos.y)*size.y-0.5f*size.y,
			0f);*/

		//transform.localPosition = Camera.main.ViewportToWorldPoint (new Vector3 (testpos.x,1f-testpos.y,Camera.main.farClipPlane));
	}
}
